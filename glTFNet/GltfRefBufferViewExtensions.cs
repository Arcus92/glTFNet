using glTFNet.IO;
using glTFNet.Specifications.Models;
using JetBrains.Annotations;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="BufferView"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefBufferViewExtensions
{
    /// <summary>
    /// Gets the buffer of the glTF buffer view.
    /// </summary>
    /// <param name="instance">The glTF buffer view reference.</param>
    /// <value>Returns the buffer.</value>
    public static GltfRef<Buffer>? Buffer(this GltfRef<BufferView> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (root.Buffers is null)
        {
            return null;
        }

        return instance.Ref(root.Buffers, instance.Data.Buffer);
    }
    
    /// <summary>
    /// Gets the buffer of the glTF buffer view.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the buffer if found.</param>
    /// <returns>Returns true, if the buffer was found.</returns>
    public static bool HasBuffer(this GltfRef<BufferView> instance, out GltfRef<Buffer> result) =>
        instance.Buffer().TryGetValue(out result);

    /// <summary>
    /// Opens the glTF buffer view.
    /// </summary>
    /// <param name="instance">The glTF buffer view reference.</param>
    /// <returns>Returns the buffer view.</returns>
    public static async Task<GltfBufferView?> Open(this GltfRef<BufferView> instance)
    {
        var buffer = instance.Buffer();
        if (!buffer.HasValue)
        {
            return null;
        }

        var loadedBuffer = await buffer.Value.Open();
        return await loadedBuffer.OpenBufferView(instance.Data);
    }
}