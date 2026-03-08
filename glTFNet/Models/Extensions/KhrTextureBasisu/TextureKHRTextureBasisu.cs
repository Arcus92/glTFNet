namespace glTFNet.Models.Extensions.KhrTextureBasisu;

/// <summary>
/// glTF extension to specify textures using the KTX v2 images with Basis Universal supercompression.
/// </summary>
[Serializable]
public class TextureKHRTextureBasisu : GlTFProperty
{
    /// <summary>
    /// The index of the image which points to a KTX v2 resource with Basis Universal supercompression.
    /// </summary>
    public int? Source { get; set; }
}