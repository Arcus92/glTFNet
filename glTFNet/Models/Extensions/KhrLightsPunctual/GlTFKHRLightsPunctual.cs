namespace glTFNet.Models.Extensions.KhrLightsPunctual;

[Serializable]
public class GlTFKHRLightsPunctual : GlTFProperty
{
    public required List<Light> Lights { get; set; }
}