namespace glTFNet.Models.Extensions.KhrXmpJsonLd;

/// <summary>
/// Metadata about the glTF asset.
/// </summary>
[Serializable]
public class GlTFKHRXmpJsonLd : GlTFProperty
{
    public required List<Dictionary<string, object>> Packets { get; set; }
}