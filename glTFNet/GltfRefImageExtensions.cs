using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Image"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefImageExtensions
{
    /// <summary>
    /// Gets the buffer view of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF image reference.</param>
    /// <value>Returns the buffer view.</value>
    public static GltfRef<BufferView>? BufferView(this GltfRef<Image> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.BufferView.HasValue || root.BufferViews is null)
        {
            return null;
        }

        return instance.Ref(root.BufferViews, instance.Data.BufferView.Value);
    }

    /// <summary>
    /// Gets the buffer view of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the buffer view if found.</param>
    /// <returns>Returns true, if the buffer view was found.</returns>
    public static bool HasBufferView(this GltfRef<Image> instance, out GltfRef<BufferView> result) =>
        instance.BufferView().TryGetValue(out result);
    
    /// <summary>
    /// Opens the image.
    /// </summary>
    /// <param name="instance">The glTF image reference.</param>
    /// <returns>Returns the image stream.</returns>
    public static async Task<Stream> Open(this GltfRef<Image> instance)
    {
        // Load from buffer
        if (instance.BufferView().TryGetValue(out var bufferView))
        {
            var loadedBufferView = await bufferView.Open();
            if (loadedBufferView is null)
            {
                throw new Exception($"Could not resolve image from buffer view: #{bufferView.Index}");
            }
            return loadedBufferView.AsStream();
        }
        
        // Load from file
        var stream = await instance.Context.As<IGltfLoaderContext>().OpenUriAsStream(instance.Data.Uri);
        if (stream is null)
        {
            throw new Exception($"Could not resolve image from uri: {instance.Data.Uri ?? "(null)"}");
        }

        return stream;
    }
}