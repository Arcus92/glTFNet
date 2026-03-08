namespace glTFNet.Models.Extensions.KhrMaterialsDispersion;

/// <summary>
/// glTF extension that defines the strength of dispersion.
/// </summary>
[Serializable]
public class MaterialKHRMaterialsDispersion : GlTFProperty
{
    /// <summary>
    /// This parameter defines dispersion in terms of the 20/Abbe number formulation.
    /// </summary>
    public float? Dispersion { get; set; }

    /// <inheritdoc cref="Dispersion"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float DispersionOrDefault => Dispersion ?? 0F;
}