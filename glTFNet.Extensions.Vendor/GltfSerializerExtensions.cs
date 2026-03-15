using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet.Extensions.Vendor;

/// <summary>
/// Extension methods for <see cref="GltfSerializer"/>.
/// </summary>
[PublicAPI]
public static class GltfSerializerExtensions
{
    /// <summary>
    /// Registers the Vendor extensions to with the glTF serializer.
    /// </summary>
    /// <param name="serializer">The serializer to register the extension to.</param>
    public static void UseVendorExtensions(this GltfSerializer serializer)
    {
        serializer.AddSerializerContext(GltfSerializerContext.Default);
    }
}