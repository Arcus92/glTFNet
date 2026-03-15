using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet.Extensions.Khronos;

/// <summary>
/// Extension methods for <see cref="GltfSerializer"/>.
/// </summary>
[PublicAPI]
public static class GltfSerializerExtensions
{
    /// <summary>
    /// Registers the Khronos extensions to with the glTF serializer.
    /// </summary>
    /// <param name="serializer">The serializer to register the extension to.</param>
    public static void UseKhronosExtensions(this GltfSerializer serializer)
    {
        serializer.AddSerializerContext(GltfSerializerContext.Default);
    }
}