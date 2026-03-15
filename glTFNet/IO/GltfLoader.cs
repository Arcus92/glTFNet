using System.Text.Json;
using glTFNet.Specifications;
using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet.IO;

/// <summary>
/// A loader class for GlTF files and binaries.
/// </summary>
[PublicAPI]
public class GltfLoader : GltfSerializer, IGltfContext, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the loaded glTF data.
    /// </summary>
    public Gltf? Data { get; private set; }
    
    /// <summary>
    /// Gets and sets the current resource resolver.
    /// </summary>
    public IResourceResolver? ResourceResolver { get; set; }

    /// <summary>
    /// Stores all open buffers by uri. The embedded buffer of GBL files has an empty uri.
    /// </summary>
    private readonly Dictionary<string, GltfBuffer> _buffers = new();
    
    /// <summary>
    /// Opens a GlTF file from the given path.
    /// This also sets the <see cref="ResourceResolver"/> to load files from the parent directory.
    /// </summary>
    /// <param name="path">The file path to open.</param>
    public async Task<GltfRef<Gltf>> Open(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (ResourceResolver is null && directory is not null)
        {
            ResourceResolver = new FileResolver(directory);
        }

        await using var stream = File.OpenRead(path);
        return await Open(stream);
    }

    /// <summary>
    /// Opens a glTF file from the given stream.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    public async Task<GltfRef<Gltf>> Open(Stream stream)
    {
        if (Data is not null)
        {
            throw new InvalidOperationException("There is already an open glTF file opened.");
        }
        
        // Ensures this stream is seekable
        if (!stream.CanSeek)
        {
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            stream = ms;
        }
        
        var header = new byte[12];
        stream.ReadExactly(header);
        var position = header.Length;
        
        var magicNumber = BitConverter.ToInt32(header);

        Gltf? gltf = null;
        
        // GLB header
        if (magicNumber == 0x46546C67)
        {
            var version = BitConverter.ToInt32(header.AsSpan()[4..]);
            var length = BitConverter.ToInt32(header.AsSpan()[8..]);

            if (version != 2)
            {
                throw new Exception($"Unsupported version of glTF: {version}");
            }
            
            // Read all chunks
            var chunkHeader = new byte[8];
            while (position < length)
            {
                stream.ReadExactly(chunkHeader);
                position += chunkHeader.Length;

                var chunkLength = BitConverter.ToInt32(chunkHeader);
                var chunkType = BitConverter.ToInt32(chunkHeader.AsSpan()[4..]);
                position += chunkLength;

                // JSON
                if (chunkType == 0x4E4F534A)
                {
                    var jsonData = new byte[chunkLength];
                    await stream.ReadExactlyAsync(jsonData);
                    gltf = JsonSerializer.Deserialize(jsonData, GltfSerializerContext.Default.Gltf);
                }
                // BIN
                else if (chunkType == 0x004E4942)
                {
                    var binData = new byte[chunkLength];
                    await stream.ReadExactlyAsync(binData);
                    _buffers.Add("", new GltfBuffer(binData));
                }
                // Unknown
                else
                {
                    stream.Seek(chunkLength, SeekOrigin.Current);
                }
            }
        }
        // JSON
        else
        {
            stream.Seek(0, SeekOrigin.Begin);
            gltf = await JsonSerializer.DeserializeAsync(stream, GltfSerializerContext.Default.Gltf);
        }

        Data = gltf ?? throw new NullReferenceException("Unable to load the glTF model.");
        return new GltfRef<Gltf>(this, gltf);
    }
    
    /// <inheritdoc />
    public T Parent<T>()
    {
        if (Data is T data)
        {
            return data;
        }
        
        throw new InvalidOperationException($"The glTF model does not contain a parent of type {typeof(T)}.");
    }
    
    /// <inheritdoc/>
    public async Task<Stream?> OpenUriAsStream(string? uri)
    {
        if (ResourceResolver is null)
        {
            return null;
        }
        return await ResourceResolver.Resolve(uri);
    }
    
    /// <inheritdoc/>
    public async Task<GltfBuffer?> OpenUriAsBuffer(string? uri)
    {
        uri ??= "";
        
        // Returns an open buffer.
        if (_buffers.TryGetValue(uri, out var buffer))
        {
            return buffer;
        }
        
        // Resolves a new binary buffer
        var stream = await OpenUriAsStream(uri);
        if (stream is null)
        {
            return null;
        }

        buffer = new GltfBuffer(stream);
        _buffers.Add(uri, buffer);
        return buffer;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var buffer in _buffers.Values)
        {
            buffer.Dispose();
        }
        _buffers.Clear();
        Data = null;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        foreach (var buffer in _buffers.Values)
        {
            await buffer.DisposeAsync();
        }
        _buffers.Clear();
        Data = null;
    }
}