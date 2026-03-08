namespace glTFNet.Models;

/// <summary>
/// The material appearance of a primitive.
/// </summary>
[Serializable]
public class Material : GlTFChildOfRootProperty
{
    /// <summary>
    /// A set of parameter values that are used to define the metallic-roughness material model from Physically Based Rendering (PBR) methodology. When undefined, all the default values of `pbrMetallicRoughness` **MUST** apply.
    /// </summary>
    public MaterialPbrMetallicRoughness? PbrMetallicRoughness { get; set; }

    /// <summary>
    /// The tangent space normal texture. The texture encodes RGB components with linear transfer function. Each texel represents the XYZ components of a normal vector in tangent space. The normal vectors use the convention +X is right and +Y is up. +Z points toward the viewer. If a fourth component (A) is present, it **MUST** be ignored. When undefined, the material does not have a tangent space normal texture.
    /// </summary>
    public MaterialNormalTextureInfo? NormalTexture { get; set; }

    /// <summary>
    /// The occlusion texture. The occlusion values are linearly sampled from the R channel. Higher values indicate areas that receive full indirect lighting and lower values indicate no indirect lighting. If other channels are present (GBA), they **MUST** be ignored for occlusion calculations. When undefined, the material does not have an occlusion texture.
    /// </summary>
    public MaterialOcclusionTextureInfo? OcclusionTexture { get; set; }

    /// <summary>
    /// The emissive texture. It controls the color and intensity of the light being emitted by the material. This texture contains RGB components encoded with the sRGB transfer function. If a fourth component (A) is present, it **MUST** be ignored. When undefined, the texture **MUST** be sampled as having `1.0` in RGB components.
    /// </summary>
    public TextureInfo? EmissiveTexture { get; set; }

    /// <summary>
    /// The factors for the emissive color of the material. This value defines linear multipliers for the sampled texels of the emissive texture.
    /// </summary>
    public System.Numerics.Vector3? EmissiveFactor { get; set; }

    /// <inheritdoc cref="EmissiveFactor"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public System.Numerics.Vector3 EmissiveFactorOrDefault => EmissiveFactor ?? new(0F, 0F, 0F);

    /// <summary>
    /// The material's alpha rendering mode enumeration specifying the interpretation of the alpha value of the base color.
    /// </summary>
    public MaterialAlphaMode? AlphaMode { get; set; }

    /// <inheritdoc cref="AlphaMode"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public MaterialAlphaMode AlphaModeOrDefault => AlphaMode ?? MaterialAlphaMode.Opaque;

    /// <summary>
    /// Specifies the cutoff threshold when in `MASK` alpha mode. If the alpha value is greater than or equal to this value then it is rendered as fully opaque, otherwise, it is rendered as fully transparent. A value greater than `1.0` will render the entire material as fully transparent. This value **MUST** be ignored for other alpha modes. When `alphaMode` is not defined, this value **MUST NOT** be defined.
    /// </summary>
    public float? AlphaCutoff { get; set; }

    /// <inheritdoc cref="AlphaCutoff"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float AlphaCutoffOrDefault => AlphaCutoff ?? 0.5F;

    /// <summary>
    /// Specifies whether the material is double sided. When this value is false, back-face culling is enabled. When this value is true, back-face culling is disabled and double-sided lighting is enabled. The back-face **MUST** have its normals reversed before the lighting equation is evaluated.
    /// </summary>
    public bool? DoubleSided { get; set; }

    /// <inheritdoc cref="DoubleSided"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool DoubleSidedOrDefault => DoubleSided ?? false;
}