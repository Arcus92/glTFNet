namespace glTFNet.Models.Extensions.Khronos.KhrMaterialsAnisotropy;

/// <summary>
/// glTF extension that defines anisotropy.
/// </summary>
[Serializable]
public class MaterialKHRMaterialsAnisotropy : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The anisotropy strength. When the anisotropy texture is present, this value is multiplied by the texture's blue channel.
    /// </summary>
    public float? AnisotropyStrength { get; set; }

    /// <inheritdoc cref="AnisotropyStrength"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float AnisotropyStrengthOrDefault => AnisotropyStrength ?? 0F;

    /// <summary>
    /// The rotation of the anisotropy in tangent, bitangent space, measured in radians counter-clockwise from the tangent. When the anisotropy texture is present, this value provides additional rotation to the vectors in the texture.
    /// </summary>
    public float? AnisotropyRotation { get; set; }

    /// <inheritdoc cref="AnisotropyRotation"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float AnisotropyRotationOrDefault => AnisotropyRotation ?? 0F;

    /// <summary>
    /// The anisotropy texture. Red and green channels represent the anisotropy direction in $[-1, 1]$ tangent, bitangent space, to be rotated by the anisotropy rotation. The blue channel contains strength as $[0, 1]$ to be multiplied by the anisotropy strength.
    /// </summary>
    public glTFNet.Models.TextureInfo? AnisotropyTexture { get; set; }
}