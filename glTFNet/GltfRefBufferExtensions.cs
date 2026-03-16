using glTFNet.IO;
using glTFNet.IO.Interfaces;
using JetBrains.Annotations;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Buffer"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefBufferExtensions
{
    /// <summary>
    /// Opens the buffer.
    /// </summary>
    /// <param name="instance">The glTF buffer reference.</param>
    /// <returns>Returns the buffer.</returns>
    public static async Task<GltfBuffer> Open(this GltfRef<Buffer> instance)
    {
        var buffer = await instance.Context.As<IGltfLoaderContext>().OpenUriAsBuffer(instance.Data.Uri);
        if (buffer is null)
        {
            throw new Exception($"Could not resolve buffer: {instance.Data.Uri ?? "(null)"}");
        }

        return buffer;
    }
}