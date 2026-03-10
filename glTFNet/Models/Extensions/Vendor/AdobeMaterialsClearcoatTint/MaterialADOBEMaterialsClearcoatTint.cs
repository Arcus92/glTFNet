namespace glTFNet.Models.Extensions.Vendor.AdobeMaterialsClearcoatTint;

/// <summary>
/// glTF extension that defines the colour tint of the clearcoat.
/// </summary>
[Serializable]
public class MaterialADOBEMaterialsClearcoatTint : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The colour of light allowed to be transmitted through the clearcoat layer of the material. A value of black means no light passes through. A value of white means all light passes through. These values are linear.
    /// </summary>
    public System.Numerics.Vector3? ClearcoatTintFactor { get; set; }

    /// <inheritdoc cref="ClearcoatTintFactor"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public System.Numerics.Vector3 ClearcoatTintFactorOrDefault => ClearcoatTintFactor ?? new(1F, 1F, 1F);

    /// <summary>
    /// The clearcoat layer tint texture.  The values are stored in sRGB.  Assume white colour if no texture is supplied.
    /// </summary>
    public glTFNet.Models.TextureInfo? ClearcoatTintTexture { get; set; }
}