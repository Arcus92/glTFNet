using glTFNet.Models;
using JetBrains.Annotations;

namespace glTFNet.IO;

/// <summary>
/// An open buffer from a <see cref="GlTFLoader"/>.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public class GlTFBuffer : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Creates a new buffer from stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public GlTFBuffer(Stream stream)
    {
        _stream = stream;
    }

    /// <summary>
    /// Creates a new buffer from a byte array.
    /// </summary>
    /// <param name="data">The byte data.</param>
    public GlTFBuffer(byte[] data)
    {
        _stream = new MemoryStream(data);
    }
    
    /// <summary>
    /// The internal data stream.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// The list of loaded buffer views.
    /// </summary>
    private readonly Dictionary<BufferView, GlTFBufferView> _bufferViews = new();
    
    /// <summary>
    /// Loads the given buffer view.
    /// </summary>
    /// <param name="bufferView">The buffer view to load.</param>
    /// <returns>Returns the loaded buffer view.</returns>
    public async Task<GlTFBufferView> OpenBufferView(BufferView bufferView)
    {
        // Get cached
        if (_bufferViews.TryGetValue(bufferView, out var loadedBufferView))
        {
            return loadedBufferView;
        }
        
        // Read the binary data
        _stream.Seek(bufferView.ByteOffsetOrDefault, SeekOrigin.Begin);
        var data = new byte[bufferView.ByteLength];
        await _stream.ReadExactlyAsync(data, 0, data.Length);

        loadedBufferView = new GlTFBufferView(data, bufferView);
        _bufferViews.Add(bufferView, loadedBufferView);
        return loadedBufferView;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _stream.Dispose();
        _bufferViews.Clear();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _stream.DisposeAsync();
        _bufferViews.Clear();
    }
}