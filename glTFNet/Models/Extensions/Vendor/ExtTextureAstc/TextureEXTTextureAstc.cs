namespace glTFNet.Models.Extensions.Vendor.ExtTextureAstc;

/// <summary>
/// glTF extension to specify textures using the KTX v2 images with ASTC compression.
/// </summary>
[Serializable]
public class TextureEXTTextureAstc : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The index of the image which points to a KTX v2 resource with ASTC compression.
    /// </summary>
    public int? Source { get; set; }
}