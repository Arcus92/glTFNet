using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet.Extensions.Archived;

/// <summary>
/// Extension methods for <see cref="GlTFSerializer"/>.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public static class GlTFSerializerExtensions
{
    /// <summary>
    /// Registers the archived extensions to with the glTF serializer.
    /// </summary>
    /// <param name="serializer">The serializer to register the extension to.</param>
    public static void UseArchivedExtensions(this GlTFSerializer serializer)
    {
        serializer.AddSerializerContext(GlTFSerializerContext.Default);
    }
}