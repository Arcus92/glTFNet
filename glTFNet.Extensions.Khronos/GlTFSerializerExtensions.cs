using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet.Extensions.Khronos;

/// <summary>
/// Extension methods for <see cref="GlTFSerializer"/>.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public static class GlTFSerializerExtensions
{
    /// <summary>
    /// Registers the Khronos extensions to with the glTF serializer.
    /// </summary>
    /// <param name="serializer">The serializer to register the extension to.</param>
    public static void UseKhronosExtensions(this GlTFSerializer serializer)
    {
        serializer.AddSerializerContext(GlTFSerializerContext.Default);
    }
}