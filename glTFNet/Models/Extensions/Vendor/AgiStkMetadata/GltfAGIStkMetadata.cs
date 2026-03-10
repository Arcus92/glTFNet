namespace glTFNet.Models.Extensions.Vendor.AgiStkMetadata;

/// <summary>
/// glTF Extension that defines metadata for use with STK (Systems Tool Kit).
/// </summary>
[Serializable]
public class GltfAGIStkMetadata : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// An array of solar panel groups.
    /// </summary>
    public List<SolarPanelGroup>? SolarPanelGroups { get; set; }
}