using glTFNet.IO;
using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;

namespace glTFNet.Tests.Helper;

public class MockContext(Gltf data) : IGltfContext, IGltfLoaderContext
{
    public Gltf Data { get; } = data;
    
    public IResourceResolver? ResourceResolver { get; set; }
    
    public T Parent<T>()
    {
        if (Data is T data)
        {
            return data;
        }
        
        throw new InvalidOperationException($"The glTF model does not contain a parent of type {typeof(T)}.");
    }

    public T As<T>()
    {
        if (this is T result)
        {
            return result;
        }
        
        throw new InvalidOperationException($"The context type {typeof(T)} could not be resolved.");
    }

    public async Task<Stream?> OpenUriAsStream(string? uri)
    {
        if (ResourceResolver is null)
        {
            return null;
        }

        return await ResourceResolver.Resolve(uri);
    }

    public async Task<GltfBuffer?> OpenUriAsBuffer(string? uri)
    {
        var stream = await OpenUriAsStream(uri);
        if (stream is null)
        {
            return null;
        }

        return new GltfBuffer(stream);
    }
}