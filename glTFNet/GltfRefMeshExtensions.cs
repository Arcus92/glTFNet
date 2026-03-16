using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Mesh"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefMeshExtensions
{
    /// <summary>
    /// Gets the primitives of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh reference.</param>
    /// <value>Returns the primitives.</value>
    public static GltfListRef<MeshPrimitive> Primitives(this GltfRef<Mesh> instance) => instance.RefList(instance.Data.Primitives);
}