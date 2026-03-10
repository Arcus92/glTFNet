namespace glTFNet.Models.Extensions.Vendor.ExtLightsImageBased;

[Serializable]
public class SceneEXTLightsImageBased : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The id of the light referenced by this scene.
    /// </summary>
    public required int Light { get; set; }
}