namespace glTFNet.Models.Extensions.Khronos.KhrXmpJsonLd;

/// <summary>
/// Metadata about the glTF asset.
/// </summary>
[Serializable]
public class GlTFKHRXmpJsonLd : glTFNet.Models.GlTFProperty
{
    public required List<Dictionary<string, object>> Packets { get; set; }
}