namespace glTFNet.Models.Extensions.Vendor.MsftTextureDds;

/// <summary>
/// glTF extension to specify textures using the DirectDraw Surface file format (DDS).
/// </summary>
[Serializable]
public class GlTFMSFTTextureDds : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The index of the images node which points to a DDS texture file.
    /// </summary>
    public int? Source { get; set; }
}