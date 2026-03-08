using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using glTFNet.Models;

namespace glTFNet.Loader;

/// <summary>
/// A loader class for GlTF files and binaries.
/// </summary>
// ReSharper disable once InconsistentNaming
public class GlTFLoader : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets and sets the current resource resolver.
    /// </summary>
    public IResourceResolver? ResourceResolver { get; set; }

    /// <summary>
    /// Stores all open buffers.
    /// </summary>
    private readonly Dictionary<string, GlTFBuffer> _buffers = new();

    /// <summary>
    /// Opens a GlTF file from the given path.
    /// This also sets the <see cref="ResourceResolver"/> to load files from the parent directory.
    /// </summary>
    /// <param name="path">The file path to open.</param>
    public async Task<GlTFRef<GlTF>> Read(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (ResourceResolver is null && directory is not null)
        {
            ResourceResolver = new FileResolver(directory);
        }

        await using var stream = File.OpenRead(path);
        return await Read(stream);
    }

    /// <summary>
    /// Opens a GlTF file from the given stream.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    public async Task<GlTFRef<GlTF>> Read(Stream stream)
    {
        // Ensures this stream is seekable
        if (!stream.CanSeek)
        {
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            stream = ms;
        }
        
        var header = new byte[12];
        stream.ReadExactly(header);
        var position = header.Length;
        
        var magicNumber = BitConverter.ToInt32(header);

        GlTF? gltf = null;
        
        // GLB header
        if (magicNumber == 0x46546C67)
        {
            var version = BitConverter.ToInt32(header[4..]);
            var length = BitConverter.ToInt32(header[8..]);

            // Read all chunks
            var chunkHeader = new byte[8];
            while (position < length)
            {
                stream.ReadExactly(chunkHeader);
                position += chunkHeader.Length;

                var chunkLength = BitConverter.ToInt32(chunkHeader);
                var chunkType = BitConverter.ToInt32(chunkHeader[4..]);
                position += chunkLength;

                // JSON
                if (chunkType == 0x4E4F534A)
                {
                    var jsonData = new byte[chunkLength];
                    await stream.ReadExactlyAsync(jsonData);
                    gltf = JsonSerializer.Deserialize(jsonData, GlTFSerializerContext.Default.GlTF);
                }
                // BIN
                else if (chunkType == 0x004E4942)
                {
                    var binData = new byte[chunkLength];
                    await stream.ReadExactlyAsync(binData);
                    _buffers.Add("", new GlTFBuffer(binData));
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
            gltf = await JsonSerializer.DeserializeAsync(stream, GlTFSerializerContext.Default.GlTF);
        }

        if (gltf == null)
        {
            throw new NullReferenceException();
        }

        return new GlTFRef<GlTF>(gltf, gltf, this);
    }
    
    /// <summary>
    /// Tries to resolve an external resource.
    /// </summary>
    /// <param name="uri">The uri to load.</param>
    /// <param name="stream">Returns the stream.</param>
    /// <returns>Returns true, if the resource could be loaded.</returns>
    public bool TryResolveStream(string? uri, [MaybeNullWhen(false)] out Stream stream)
    {
        stream = null;
        return ResourceResolver is not null && ResourceResolver.TryResolve(uri, out stream);
    }
    
    /// <summary>
    /// Tries to resolve an external binary buffer.
    /// </summary>
    /// <param name="uri">The uri to load.</param>
    /// <param name="buffer">Returns the buffer.</param>
    /// <returns>Returns true, if the binary buffer could be loaded.</returns>
    public bool TryResolveBuffer(string? uri, [MaybeNullWhen(false)] out GlTFBuffer buffer)
    {
        // Returns an open buffer.
        if (_buffers.TryGetValue(uri ?? "", out buffer))
        {
            return true;
        }
        
        // Resolves a new binary buffer
        if (!TryResolveStream(uri, out var stream))
        {
            buffer = null;
            return false;
        }

        buffer = new GlTFBuffer(stream);
        _buffers.Add(uri ?? "", buffer);
        return true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var buffer in _buffers.Values)
        {
            buffer.Dispose();
        }
        _buffers.Clear();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        foreach (var buffer in _buffers.Values)
        {
            await buffer.DisposeAsync();
        }
        _buffers.Clear();
    }
}