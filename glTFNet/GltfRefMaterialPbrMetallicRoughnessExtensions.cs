using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="MaterialPbrMetallicRoughness"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefMaterialPbrMetallicRoughnessExtensions
{
    /// <summary>
    /// Gets the base color texture info.
    /// </summary>
    /// <param name="instance">The glTF pbr material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GltfRef<TextureInfo>? BaseColorTexture(this GltfRef<MaterialPbrMetallicRoughness> instance)
    {
        if (instance.Data.BaseColorTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.BaseColorTexture);
    }
    
    /// <summary>
    /// Gets the base color texture info.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasBaseColorTexture(this GltfRef<MaterialPbrMetallicRoughness> instance, out GltfRef<TextureInfo> result) =>
        instance.BaseColorTexture().TryGetValue(out result);

    /// <summary>
    /// Gets the metallic roughness texture info.
    /// </summary>
    /// <param name="instance">The glTF pbr material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GltfRef<TextureInfo>? MetallicRoughnessTexture(this GltfRef<MaterialPbrMetallicRoughness> instance)
    {
        if (instance.Data.MetallicRoughnessTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.MetallicRoughnessTexture);
    }
    
    /// <summary>
    /// Gets the metallic roughness texture info.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture info if found.</param>
    /// <returns>Returns true, if the texture info was found.</returns>
    public static bool HasMetallicRoughnessTexture(this GltfRef<MaterialPbrMetallicRoughness> instance, out GltfRef<TextureInfo> result) =>
        instance.MetallicRoughnessTexture().TryGetValue(out result);
}