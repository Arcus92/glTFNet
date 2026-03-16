using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Node"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefNodeExtensions
{
    /// <summary>
    /// Gets the children of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the child nodes.</value>
    public static GltfIndexedListRef<Node> Children(this GltfRef<Node> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (instance.Data.Children is null || root.Nodes is null)
        {
            return GltfIndexedListRef<Node>.Empty;
        }

        return instance.RefIndexedList(root.Nodes, instance.Data.Children);
    }

    /// <summary>
    /// Gets the mesh of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the mesh.</value>
    public static GltfRef<Mesh>? Mesh(this GltfRef<Node> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Mesh.HasValue || root.Meshes is null)
        {
            return null;
        }

        return instance.Ref(root.Meshes, instance.Data.Mesh.Value);
    }

    /// <summary>
    /// Gets the mesh of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the mesh if found.</param>
    /// <returns>Returns true, if the mesh was found.</returns>
    public static bool HasMesh(this GltfRef<Node> instance, out GltfRef<Mesh> result) =>
        instance.Mesh().TryGetValue(out result);
    
    /// <summary>
    /// Gets the camera of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the camera.</value>
    public static GltfRef<Camera>? Camera(this GltfRef<Node> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Camera.HasValue || root.Cameras is null)
        {
            return null;
        }

        return instance.Ref(root.Cameras, instance.Data.Camera.Value);
    }
    
    /// <summary>
    /// Gets the camera of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the camera if found.</param>
    /// <returns>Returns true, if the camera was found.</returns>
    public static bool HasCamera(this GltfRef<Node> instance, out GltfRef<Camera> result) =>
        instance.Camera().TryGetValue(out result);
}