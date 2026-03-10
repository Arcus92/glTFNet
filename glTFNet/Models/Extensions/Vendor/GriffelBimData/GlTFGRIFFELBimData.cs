namespace glTFNet.Models.Extensions.Vendor.GriffelBimData;

/// <summary>
/// Domain specific properties for glTF nodes.
/// </summary>
[Serializable]
public class GlTFGRIFFELBimData
{
    /// <summary>
    /// Collection of unique property values.
    /// </summary>
    public required List<string> PropertyValues { get; set; }

    /// <summary>
    /// Collection of unique property names.
    /// </summary>
    public required List<string> PropertyNames { get; set; }

    /// <summary>
    /// Collection of unique property name - property value pairs.
    /// </summary>
    public required List<object> Properties { get; set; }

    /// <summary>
    /// Collection of types - common sets of properties for many nodes.
    /// </summary>
    public List<object>? Types { get; set; }
}