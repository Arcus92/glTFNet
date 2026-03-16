using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Texture"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefTextureExtensions
{
    /// <summary>
    /// Gets the image of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF texture reference.</param>
    /// <value>Returns the source image.</value>
    public static GltfRef<Image>? Source(this GltfRef<Texture> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Source.HasValue || root.Images is null)
        {
            return null;
        }

        return instance.Ref(root.Images, instance.Data.Source.Value);
    }
    
    /// <summary>
    /// Gets the image of the glTF texture reference.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the image if found.</param>
    /// <returns>Returns true, if the image was found.</returns>
    public static bool HasSource(this GltfRef<Texture> instance, out GltfRef<Image> result) =>
        instance.Source().TryGetValue(out result);

    /// <summary>
    /// Gets the texture sampler of the glTF texture reference.
    /// </summary>
    /// <param name="instance">The glTF texture reference.</param>
    /// <value>Returns the sampler.</value>
    public static GltfRef<Sampler>? Sampler(this GltfRef<Texture> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Sampler.HasValue || root.Samplers is null)
        {
            return null;
        }

        return instance.Ref(root.Samplers, instance.Data.Sampler.Value);
    }
    
    /// <summary>
    /// Gets the texture sampler of the glTF texture reference.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the sampler if found.</param>
    /// <returns>Returns true, if the sampler was found.</returns>
    public static bool HasSampler(this GltfRef<Texture> instance, out GltfRef<Sampler> result) =>
        instance.Sampler().TryGetValue(out result);
}