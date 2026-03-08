using glTFNet.Models;

namespace glTFNet.Loader;

/// <summary>
/// An open buffer from a <see cref="GlTFLoader"/>.
/// </summary>
/// <param name="stream">The buffer stream.</param>
// ReSharper disable once InconsistentNaming
public class GlTFBuffer(Stream stream) : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// The internal data stream.
    /// </summary>
    private readonly Stream _stream = stream;

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