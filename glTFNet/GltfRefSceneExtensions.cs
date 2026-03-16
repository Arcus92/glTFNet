using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Scene"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefSceneExtensions
{
    /// <summary>
    /// Gets the root scene of the glTF scene.
    /// </summary>
    /// <param name="instance">The glTF scene reference.</param>
    /// <value>Returns the scene nodes.</value>
    public static GltfIndexedListRef<Node> Nodes(this GltfRef<Scene> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (instance.Data.Nodes is null || root.Nodes is null)
        {
            return GltfIndexedListRef<Node>.Empty;
        }

        return instance.RefIndexedList(root.Nodes, instance.Data.Nodes);
    }
}