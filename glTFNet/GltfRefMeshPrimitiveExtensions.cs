using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="MeshPrimitive"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefMeshPrimitiveExtensions
{
    /// <summary>
    /// Gets the buffer accessor for the indices of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    /// <value>Returns the indices accessor.</value>
    public static GltfRef<Accessor>? Indices(this GltfRef<MeshPrimitive> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Indices.HasValue || root.Accessors is null)
        {
            return null;
        }

        return instance.Ref(root.Accessors, instance.Data.Indices.Value);
    }
    
    /// <summary>
    /// Gets the buffer accessor for the indices of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the accessor if found.</param>
    /// <returns>Returns true, if the accessor was found.</returns>
    public static bool HasIndices(this GltfRef<MeshPrimitive> instance, out GltfRef<Accessor> result) =>
        instance.Indices().TryGetValue(out result);

    /// <summary>
    /// Gets all attributes as name/accessor pair of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    public static GltfIndexedDictionaryRef<string, Accessor> Attributes(this GltfRef<MeshPrimitive> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (root.Accessors is null)
        {
            return GltfIndexedDictionaryRef<string, Accessor>.Empty;
        }

        return instance.RefIndexedDictionary(root.Accessors, instance.Data.Attributes);
    }

    /// <summary>
    /// Gets the attribute with the given name of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="attributeName">The attribute name to find.</param>
    /// <param name="result">Returns the accessor if found.</param>
    /// <returns>Returns true, if the accessor was found.</returns>
    public static bool HasAttribute(this GltfRef<MeshPrimitive> instance, string attributeName, out GltfRef<Accessor> result) =>
        instance.Attributes().TryGetValue(attributeName, out result);

    /// <summary>
    /// Gets the material of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    /// <value>Returns the material.</value>
    public static GltfRef<Material>? Material(this GltfRef<MeshPrimitive> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Material.HasValue || root.Materials is null)
        {
            return null;
        }

        return instance.Ref(root.Materials, instance.Data.Material.Value);
    }
    
    /// <summary>
    /// Gets the material of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the material if found.</param>
    /// <returns>Returns true, if the material was found.</returns>
    public static bool HasMaterial(this GltfRef<MeshPrimitive> instance, out GltfRef<Material> result) =>
        instance.Material().TryGetValue(out result);
}