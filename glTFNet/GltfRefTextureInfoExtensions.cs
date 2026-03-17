using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="TextureInfo"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefTextureInfoExtensions
{
    /// <summary>
    /// Gets the texture from the glTF texture info.
    /// </summary>
    /// <param name="instance">The glTF texture info reference.</param>
    /// <value>Returns the texture.</value>
    public static GltfRef<Texture>? Texture<T>(this GltfRef<T> instance) where T : TextureInfo
    {
        var root = instance.Context.Parent<Gltf>();
        if (root.Textures is null)
        {
            return null;
        }

        return instance.Ref(root.Textures, instance.Data.Index);
    }
    
    /// <summary>
    /// Gets the texture of the glTF texture info.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the texture if found.</param>
    /// <returns>Returns true, if the texture was found.</returns>
    public static bool HasTexture<T>(this GltfRef<T> instance, out GltfRef<Texture> result) where T : TextureInfo =>
        instance.Texture().TryGetValue(out result);
}