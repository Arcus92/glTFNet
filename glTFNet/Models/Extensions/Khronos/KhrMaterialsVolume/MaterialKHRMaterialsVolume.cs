namespace glTFNet.Models.Extensions.Khronos.KhrMaterialsVolume;

/// <summary>
/// glTF extension that defines the parameters for the volume of a material.
/// </summary>
[Serializable]
public class MaterialKHRMaterialsVolume : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The thickness of the volume beneath the surface. The value is given in the coordinate space of the mesh. A value greater than 0 turns the mesh into a volume with a homogeneous medium, enabling refraction, absorption and subsurface scattering. The actual value may be ignored by renderers that are able to derive the thickness from the mesh (ray tracer).
    /// </summary>
    public float? ThicknessFactor { get; set; }

    /// <inheritdoc cref="ThicknessFactor"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float ThicknessFactorOrDefault => ThicknessFactor ?? 0F;

    /// <summary>
    /// A texture that defines the thickness of the volume, stored in the G channel. Will be multiplied by thicknessFactor.
    /// </summary>
    public glTFNet.Models.TextureInfo? ThicknessTexture { get; set; }

    /// <summary>
    /// Density of the medium given as the average distance that light travels in the medium before interacting with a particle. The value is given in world space. When undefined, the value is assumed to be infinite.
    /// </summary>
    public float? AttenuationDistance { get; set; }

    /// <summary>
    /// Color that white light turns into due to absorption when reaching the attenuation distance.
    /// </summary>
    public System.Numerics.Vector3? AttenuationColor { get; set; }

    /// <inheritdoc cref="AttenuationColor"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public System.Numerics.Vector3 AttenuationColorOrDefault => AttenuationColor ?? new(1F, 1F, 1F);
}