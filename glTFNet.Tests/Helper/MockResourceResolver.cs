using glTFNet.IO.Interfaces;

namespace glTFNet.Tests.Helper;

public class MockResourceResolver : IResourceResolver
{
    private readonly Dictionary<string, byte[]> _buffers = new();

    public MockResourceResolver Add(string uri, byte[] buffer)
    {
        _buffers.Add(uri, buffer);
        return this;
    }
    
    public MockResourceResolver Add(byte[] buffer)
    {
        _buffers.Add("", buffer);
        return this;
    }
    
    public Task<Stream?> Resolve(string? uri)
    {
        uri ??= "";

        if (_buffers.TryGetValue(uri, out var buffer))
        {
            return Task.FromResult<Stream?>(new MemoryStream(buffer));
        }

        return Task.FromResult<Stream?>(null);
    }
}