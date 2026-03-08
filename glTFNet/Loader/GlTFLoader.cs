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
        var gltf = await JsonSerializer.DeserializeAsync(stream, GlTFSerializerContext.Default.GlTF);
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
    private bool TryResolve(string? uri, [MaybeNullWhen(false)] out Stream stream)
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
        if (!TryResolve(uri, out var stream))
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