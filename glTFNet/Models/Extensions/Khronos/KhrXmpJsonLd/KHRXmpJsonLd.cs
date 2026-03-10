namespace glTFNet.Models.Extensions.Khronos.KhrXmpJsonLd;

/// <summary>
/// References an XMP packet listed in `KHR_xmp_json_ld glTF extension`
/// </summary>
[Serializable]
public class KHRXmpJsonLd : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The id of the XMP packet referenced.
    /// </summary>
    public required int Packet { get; set; }
}