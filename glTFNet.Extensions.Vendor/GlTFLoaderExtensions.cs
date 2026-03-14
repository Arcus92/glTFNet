using glTFNet.IO;

namespace glTFNet.Extensions.Vendor;

/// <summary>
/// Extension methods for <see cref="GlTFLoader"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class GlTFLoaderExtensions
{
    /// <summary>
    /// Registers the Vendor extensions to with the glTF loader.
    /// </summary>
    /// <param name="loader">The loader to register the extension to.</param>
    public static void UseVendorExtensions(this GlTFLoader loader)
    {
        loader.AddSerializerContext(GlTFSerializerContext.Default);
    }
}