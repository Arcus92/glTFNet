using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Material"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefMaterialExtensions
{
    /// <summary>
    /// Gets the pbr material properties of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the pbr material properties.</value>
    public static GltfRef<MaterialPbrMetallicRoughness>? PbrMetallicRoughness(this GltfRef<Material> instance)
    {
        if (instance.Data.PbrMetallicRoughness is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.PbrMetallicRoughness);
    }
    
    /// <summary>
    /// Gets the pbr material properties of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasPbrMetallicRoughness(this GltfRef<Material> instance, out GltfRef<MaterialPbrMetallicRoughness> result) =>
        instance.PbrMetallicRoughness().TryGetValue(out result);

    /// <summary>
    /// Gets the normal texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GltfRef<MaterialNormalTextureInfo>? NormalTexture(this GltfRef<Material> instance)
    {
        if (instance.Data.NormalTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.NormalTexture);
    }

    /// <summary>
    /// Gets the normal texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasNormalTexture(this GltfRef<Material> instance, out GltfRef<MaterialNormalTextureInfo> result) =>
        instance.NormalTexture().TryGetValue(out result);
    
    /// <summary>
    /// Gets the occlusion texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GltfRef<MaterialOcclusionTextureInfo>? OcclusionTexture(this GltfRef<Material> instance)
    {
        if (instance.Data.OcclusionTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.OcclusionTexture);
    }
    
    /// <summary>
    /// Gets the occlusion texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasOcclusionTexture(this GltfRef<Material> instance, out GltfRef<MaterialOcclusionTextureInfo> result) =>
        instance.OcclusionTexture().TryGetValue(out result);

    /// <summary>
    /// Gets the emissive texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GltfRef<TextureInfo>? EmissiveTexture(this GltfRef<Material> instance)
    {
        if (instance.Data.EmissiveTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.EmissiveTexture);
    }
    
    /// <summary>
    /// Gets the emissive texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasEmissiveTexture(this GltfRef<Material> instance, out GltfRef<TextureInfo> result) =>
        instance.EmissiveTexture().TryGetValue(out result);
}