namespace glTFNet.Models.Extensions.KhrLightsPunctual;

[Serializable]
public class NodeKHRLightsPunctual : GlTFProperty
{
    /// <summary>
    /// The id of the light referenced by this node.
    /// </summary>
    public required int Light { get; set; }
}