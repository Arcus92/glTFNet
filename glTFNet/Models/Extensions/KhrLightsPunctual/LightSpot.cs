namespace glTFNet.Models.Extensions.KhrLightsPunctual;

[Serializable]
public class LightSpot : GlTFProperty
{
    /// <summary>
    /// Angle in radians from centre of spotlight where falloff begins.
    /// </summary>
    public float? InnerConeAngle { get; set; }

    /// <inheritdoc cref="InnerConeAngle"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float InnerConeAngleOrDefault => InnerConeAngle ?? 0F;

    /// <summary>
    /// Angle in radians from centre of spotlight where falloff ends.
    /// </summary>
    public float? OuterConeAngle { get; set; }

    /// <inheritdoc cref="OuterConeAngle"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float OuterConeAngleOrDefault => OuterConeAngle ?? 0.7853982F;
}