namespace glTFNet.Models.Extensions.KhrMaterialsEmissiveStrength;

/// <summary>
/// glTF extension that adjusts the strength of emissive material properties.
/// </summary>
[Serializable]
public class MaterialKHRMaterialsEmissiveStrength : GlTFProperty
{
    /// <summary>
    /// The strength adjustment to be multiplied with the material's emissive value.
    /// </summary>
    public float? EmissiveStrength { get; set; }

    /// <inheritdoc cref="EmissiveStrength"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float EmissiveStrengthOrDefault => EmissiveStrength ?? 1F;
}