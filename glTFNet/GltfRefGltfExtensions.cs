using glTFNet.Specifications.Models;
using JetBrains.Annotations;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Gltf"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefGltfExtensions
{
    /// <summary>
    /// Gets the asset of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfRef<Asset>? Asset(this GltfRef<Gltf> instance) => instance.Ref(instance.Data.Asset);

    /// <summary>
    /// Gets the root scene of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <value>Returns the scene if found.</value>
    public static GltfRef<Scene>? Scene(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.Scene.HasValue || root.Scenes is null)
        {
            return null;
        }

        return instance.Ref(root.Scenes, instance.Data.Scene.Value);
    }
    
    /// <summary>
    /// Gets the root scene of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the scene if found.</param>
    /// <returns>Returns true, if the scene was found.</returns>
    public static bool HasScene(this GltfRef<Gltf> instance, out GltfRef<Scene> result) => 
        instance.Scene().TryGetValue(out result);

    /// <summary>
    /// Gets all scenes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Scene> Scenes(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Scenes is null ? GltfListRef<Scene>.Empty : instance.RefList(root.Scenes);
    }

    /// <summary>
    /// Gets all nodes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Node> Nodes(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Nodes is null ? GltfListRef<Node>.Empty : instance.RefList(root.Nodes);
    }

    /// <summary>
    /// Gets all cameras of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Camera> Cameras(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Cameras is null ? GltfListRef<Camera>.Empty : instance.RefList(root.Cameras);
    }

    /// <summary>
    /// Gets all materials of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Material> Materials(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Materials is null ? GltfListRef<Material>.Empty : instance.RefList(root.Materials);
    }

    /// <summary>
    /// Gets all textures of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Texture> Textures(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Textures is null ? GltfListRef<Texture>.Empty : instance.RefList(root.Textures);
    }

    /// <summary>
    /// Gets all samplers of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Sampler> Samplers(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Samplers is null ? GltfListRef<Sampler>.Empty : instance.RefList(root.Samplers);
    }

    /// <summary>
    /// Gets all images of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Image> Images(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Images is null ? GltfListRef<Image>.Empty : instance.RefList(root.Images);
    }

    /// <summary>
    /// Gets all buffers of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Buffer> Buffers(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Buffers is null ? GltfListRef<Buffer>.Empty : instance.RefList(root.Buffers);
    }

    /// <summary>
    /// Gets all buffer views of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<BufferView> BufferViews(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.BufferViews is null ? GltfListRef<BufferView>.Empty : instance.RefList(root.BufferViews);
    }

    /// <summary>
    /// Gets all accessors of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Accessor> Accessors(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Accessors is null ? GltfListRef<Accessor>.Empty : instance.RefList(root.Accessors);
    }

    /// <summary>
    /// Gets all meshes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Mesh> Meshes(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Meshes is null ? GltfListRef<Mesh>.Empty : instance.RefList(root.Meshes);
    }

    /// <summary>
    /// Gets all skins of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Skin> Skins(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Skins is null ? GltfListRef<Skin>.Empty : instance.RefList(root.Skins);
    }

    /// <summary>
    /// Gets all animations of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GltfListRef<Animation> Animations(this GltfRef<Gltf> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        return root.Animations is null ? GltfListRef<Animation>.Empty : instance.RefList(root.Animations);
    }
}