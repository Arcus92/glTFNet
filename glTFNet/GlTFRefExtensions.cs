using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.IO;
using glTFNet.Models;
using JetBrains.Annotations;
using Buffer = glTFNet.Models.Buffer;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GlTFRefExtensions"/>.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public static class GlTFRefExtensions
{
    /// <summary>
    /// Tries to unwrap the nullable glTF reference.
    /// </summary>
    /// <param name="result">Returns the unwrapped reference.</param>
    /// <param name="instance">The glTF reference.</param>
    /// <returns>Returns true, if this nullable wrapper has a value.</returns>
    public static bool TryGet<T>(this GlTFRef<T>? instance, out GlTFRef<T> result)
    {
        if (!instance.HasValue)
        {
            result = default;
            return false;
        }

        result = instance.Value;
        return true;
    }

    /// <summary>
    /// Gets the asset of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFRef<Asset>? Asset(this GlTFRef<GlTF> instance) => instance.Ref(instance.Data.Asset);

    /// <summary>
    /// Gets the root scene of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <value>Returns the scene if found.</value>
    public static GlTFRef<Scene>? Scene(this GlTFRef<GlTF> instance)
    {
        if (!instance.Data.Scene.HasValue || instance.Root.Scenes is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Scenes, instance.Data.Scene.Value);
    }

    /// <summary>
    /// Gets all scenes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Scene> Scenes(this GlTFRef<GlTF> instance) => instance.Root.Scenes is null ? GlTFListRef<Scene>.Empty : instance.RefList(instance.Root.Scenes);

    /// <summary>
    /// Gets all nodes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Node> Nodes(this GlTFRef<GlTF> instance) => instance.Root.Nodes is null ? GlTFListRef<Node>.Empty : instance.RefList(instance.Root.Nodes);

    /// <summary>
    /// Gets all cameras of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Camera> Camera(this GlTFRef<GlTF> instance) => instance.Root.Cameras is null ? GlTFListRef<Camera>.Empty : instance.RefList(instance.Root.Cameras);

    /// <summary>
    /// Gets all materials of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Material> Materials(this GlTFRef<GlTF> instance) => instance.Root.Materials is null ? GlTFListRef<Material>.Empty : instance.RefList(instance.Root.Materials);

    /// <summary>
    /// Gets all textures of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Texture> Textures(this GlTFRef<GlTF> instance) => instance.Root.Textures is null ? GlTFListRef<Texture>.Empty : instance.RefList(instance.Root.Textures);

    /// <summary>
    /// Gets all samplers of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Sampler> Samplers(this GlTFRef<GlTF> instance) => instance.Root.Samplers is null ? GlTFListRef<Sampler>.Empty : instance.RefList(instance.Root.Samplers);

    /// <summary>
    /// Gets all images of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Image> Images(this GlTFRef<GlTF> instance) => instance.Root.Images is null ? GlTFListRef<Image>.Empty : instance.RefList(instance.Root.Images);

    /// <summary>
    /// Gets all buffers of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Buffer> Buffers(this GlTFRef<GlTF> instance) => instance.Root.Buffers is null ? GlTFListRef<Buffer>.Empty : instance.RefList(instance.Root.Buffers);

    /// <summary>
    /// Gets all buffer views of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<BufferView> BufferViews(this GlTFRef<GlTF> instance) => instance.Root.BufferViews is null ? GlTFListRef<BufferView>.Empty : instance.RefList(instance.Root.BufferViews);

    /// <summary>
    /// Gets all accessors of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Accessor> Accessors(this GlTFRef<GlTF> instance) => instance.Root.Accessors is null ? GlTFListRef<Accessor>.Empty : instance.RefList(instance.Root.Accessors);

    /// <summary>
    /// Gets all meshes of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Mesh> Meshes(this GlTFRef<GlTF> instance) => instance.Root.Meshes is null ? GlTFListRef<Mesh>.Empty : instance.RefList(instance.Root.Meshes);

    /// <summary>
    /// Gets all skins of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Skin> Skins(this GlTFRef<GlTF> instance) => instance.Root.Skins is null ? GlTFListRef<Skin>.Empty : instance.RefList(instance.Root.Skins);

    /// <summary>
    /// Gets all animations of the glTF root.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    public static GlTFListRef<Animation> Animations(this GlTFRef<GlTF> instance) => instance.Root.Animations is null ? GlTFListRef<Animation>.Empty : instance.RefList(instance.Root.Animations);

    /// <summary>
    /// Gets the root scene of the glTF scene.
    /// </summary>
    /// <param name="instance">The glTF scene reference.</param>
    /// <value>Returns the scene nodes.</value>
    public static GlTFIndexedListRef<Node> Nodes(this GlTFRef<Scene> instance)
    {
        if (instance.Data.Nodes is null || instance.Root.Nodes is null)
        {
            return GlTFIndexedListRef<Node>.Empty;
        }

        return instance.RefIndexedList(instance.Root.Nodes, instance.Data.Nodes);
    }

    /// <summary>
    /// Gets the children of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the child nodes.</value>
    public static GlTFIndexedListRef<Node> Children(this GlTFRef<Node> instance)
    {
        if (instance.Data.Children is null || instance.Root.Nodes is null)
        {
            return GlTFIndexedListRef<Node>.Empty;
        }

        return instance.RefIndexedList(instance.Root.Nodes, instance.Data.Children);
    }

    /// <summary>
    /// Gets the mesh of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the mesh.</value>
    public static GlTFRef<Mesh>? Mesh(this GlTFRef<Node> instance)
    {
        if (!instance.Data.Mesh.HasValue || instance.Root.Meshes is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Meshes, instance.Data.Mesh.Value);
    
    }

    /// <summary>
    /// Gets the camera of the glTF node.
    /// </summary>
    /// <param name="instance">The glTF node reference.</param>
    /// <value>Returns the camera.</value>
    public static GlTFRef<Camera>? Camera(this GlTFRef<Node> instance)
    {
        if (!instance.Data.Camera.HasValue || instance.Root.Cameras is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Cameras, instance.Data.Camera.Value);
    }

    /// <summary>
    /// Gets the primitives of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh reference.</param>
    /// <value>Returns the primitives.</value>
    public static GlTFListRef<MeshPrimitive> Primitives(this GlTFRef<Mesh> instance) => instance.RefList(instance.Data.Primitives);

    /// <summary>
    /// Gets the buffer accessor for the indices of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    /// <value>Returns the indices accessor.</value>
    public static GlTFRef<Accessor>? Indices(this GlTFRef<MeshPrimitive> instance)
    {
        if (!instance.Data.Indices.HasValue || instance.Root.Accessors is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Accessors, instance.Data.Indices.Value);
    }

    /// <summary>
    /// Gets all attributes as name/accessor pair of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    public static GlTFIndexedDictionaryRef<string, Accessor> Attributes(this GlTFRef<MeshPrimitive> instance)
    {
        if (instance.Root.Accessors is null)
        {
            return GlTFIndexedDictionaryRef<string, Accessor>.Empty;
        }

        return instance.RefIndexedDictionary(instance.Root.Accessors, instance.Data.Attributes);
    }

    /// <summary>
    /// Gets the material of the glTF mesh.
    /// </summary>
    /// <param name="instance">The glTF mesh primitive reference.</param>
    /// <value>Returns the material.</value>
    public static GlTFRef<Material>? Material(this GlTFRef<MeshPrimitive> instance)
    {
        if (!instance.Data.Material.HasValue || instance.Root.Materials is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Materials, instance.Data.Material.Value);
    }

    /// <summary>
    /// Gets the buffer view of the glTF accessor.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <value>Returns the buffer view.</value>
    public static GlTFRef<BufferView>? BufferView(this GlTFRef<Accessor> instance)
    {
        if (!instance.Data.BufferView.HasValue || instance.Root.BufferViews is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.BufferViews, instance.Data.BufferView.Value);
    }

    /// <summary>
    /// Reads the data from the glTF accessor.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <remarks>
    /// Only Scalar is fully supported! Vec2, Vec3, Vec4 and Mat4x4 only support float values. Mat2x2 and Mat3x3
    /// are not supported.<br/>
    /// You can use <see cref="Read{T}"/> to pass a custom struct with the given length to bypass this limitation.
    /// </remarks>
    /// <returns>Returns the data.</returns>
    public static async Task<Array?> Read(this GlTFRef<Accessor> instance)
    {
        var bufferView = instance.BufferView();
        if (!bufferView.HasValue)
        {
            return null;
        }

        // Loads the buffer view
        var loadedBufferView = await bufferView.Value.Open();
        return loadedBufferView?.Read(instance.Data);
    }

    /// <summary>
    /// Reads the data from the glTF accessor as the given type.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <remarks>
    /// The component type must match the component size in the accessor.
    /// </remarks>
    /// <typeparam name="T">The component type to read.</typeparam>
    /// <returns>Returns the data.</returns>
    public static async Task<T[]?> Read<T>(this GlTFRef<Accessor> instance) where T : struct
    {
        var bufferView = instance.BufferView();
        if (!bufferView.HasValue)
        {
            return null;
        }

        // Loads the buffer view
        var loadedBufferView = await bufferView.Value.Open();
        return loadedBufferView?.Read<T>(instance.Data);
    }

    /// <summary>
    /// Gets the buffer of the glTF buffer view.
    /// </summary>
    /// <param name="instance">The glTF buffer view reference.</param>
    /// <value>Returns the buffer.</value>
    public static GlTFRef<Buffer>? Buffer(this GlTFRef<BufferView> instance)
    {
        if (instance.Root.Buffers is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Buffers, instance.Data.Buffer);
    }

    /// <summary>
    /// Opens the glTF buffer view.
    /// </summary>
    /// <param name="instance">The glTF buffer view reference.</param>
    /// <returns>Returns the buffer view.</returns>
    public static async Task<GlTFBufferView?> Open(this GlTFRef<BufferView> instance)
    {
        var buffer = instance.Buffer();
        if (!buffer.HasValue)
        {
            return null;
        }

        var loadedBuffer = await buffer.Value.Open();
        return await loadedBuffer.OpenBufferView(instance.Data);
    }

    /// <summary>
    /// Opens the buffer.
    /// </summary>
    /// <param name="instance">The glTF buffer reference.</param>
    /// <returns>Returns the buffer.</returns>
    public static async Task<GlTFBuffer> Open(this GlTFRef<Buffer> instance)
    {
        var buffer = await instance.Loader.OpenUriAsBuffer(instance.Data.Uri);
        if (buffer is null)
        {
            throw new Exception($"Could not resolve buffer: {instance.Data.Uri ?? "(null)"}");
        }

        return buffer;
    }


    /// <summary>
    /// Gets the pbr material properties of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the pbr material properties.</value>
    public static GlTFRef<MaterialPbrMetallicRoughness>? PbrMetallicRoughness(this GlTFRef<Material> instance)
    {
        if (instance.Data.PbrMetallicRoughness is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.PbrMetallicRoughness);
    }

    /// <summary>
    /// Gets the normal texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GlTFRef<MaterialNormalTextureInfo>? NormalTexture(this GlTFRef<Material> instance)
    {
        if (instance.Data.NormalTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.NormalTexture);
    }

    /// <summary>
    /// Gets the occlusion texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GlTFRef<MaterialOcclusionTextureInfo>? OcclusionTexture(this GlTFRef<Material> instance)
    {
        if (instance.Data.OcclusionTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.OcclusionTexture);
    }

    /// <summary>
    /// Gets the emissive texture info of the glTF material.
    /// </summary>
    /// <param name="instance">The glTF material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GlTFRef<TextureInfo>? EmissiveTexture(this GlTFRef<Material> instance)
    {
        if (instance.Data.EmissiveTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.EmissiveTexture);
    }

    /// <summary>
    /// Gets the base color texture info.
    /// </summary>
    /// <param name="instance">The glTF pbr material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GlTFRef<TextureInfo>? BaseColorTexture(this GlTFRef<MaterialPbrMetallicRoughness> instance)
    {
        if (instance.Data.BaseColorTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.BaseColorTexture);
    }

    /// <summary>
    /// Gets the metallic roughness texture info.
    /// </summary>
    /// <param name="instance">The glTF pbr material reference.</param>
    /// <value>Returns the texture info.</value>
    public static GlTFRef<TextureInfo>? MetallicRoughnessTexture(this GlTFRef<MaterialPbrMetallicRoughness> instance)
    {
        if (instance.Data.MetallicRoughnessTexture is null)
        {
            return null;
        }

        return instance.Ref(instance.Data.MetallicRoughnessTexture);
    }

    /// <summary>
    /// Gets the texture from the texture info.
    /// </summary>
    /// <param name="instance">The glTF texture info reference.</param>
    /// <value>Returns the texture.</value>
    public static GlTFRef<Texture>? Texture<T>(this GlTFRef<T> instance) where T : TextureInfo
    {
        if (instance.Root.Textures is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Textures, instance.Data.Index);
    }

    /// <summary>
    /// Gets the image of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF texture reference.</param>
    /// <value>Returns the source image.</value>
    public static GlTFRef<Image>? Source(this GlTFRef<Texture> instance)
    {
        if (!instance.Data.Source.HasValue || instance.Root.Images is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Images, instance.Data.Source.Value);
    }

    /// <summary>
    /// Gets the texture sampler of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF texture reference.</param>
    /// <value>Returns the sampler.</value>
    public static GlTFRef<Sampler>? Sampler(this GlTFRef<Texture> instance)
    {
        if (!instance.Data.Sampler.HasValue || instance.Root.Samplers is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.Samplers, instance.Data.Sampler.Value);
    }

    /// <summary>
    /// Gets the buffer view of the glTF image.
    /// </summary>
    /// <param name="instance">The glTF image reference.</param>
    /// <value>Returns the buffer view.</value>
    public static GlTFRef<BufferView>? BufferView(this GlTFRef<Image> instance)
    {
        if (!instance.Data.BufferView.HasValue || instance.Root.BufferViews is null)
        {
            return null;
        }

        return instance.Ref(instance.Root.BufferViews, instance.Data.BufferView.Value);
    }

    /// <summary>
    /// Opens the image.
    /// </summary>
    /// <param name="instance">The glTF image reference.</param>
    /// <returns>Returns the image stream.</returns>
    public static async Task<Stream> Open(this GlTFRef<Image> instance)
    {
        // Load from buffer
        if (instance.BufferView().TryGet(out var bufferView))
        {
            var loadedBufferView = await bufferView.Open();
            if (loadedBufferView is null)
            {
                throw new Exception($"Could not resolve image from buffer view: #{bufferView.Index}");
            }
            return loadedBufferView.AsStream();
        }
            
        // Load from file
        var stream = await instance.Loader.OpenUriAsStream(instance.Data.Uri);
        if (stream is null)
        {
            throw new Exception($"Could not resolve image from uri: {instance.Data.Uri ?? "(null)"}");
        }

        return stream;
    }

    /// <summary>
    /// Tries to get an extension object by the given name.
    /// </summary>
    /// <param name="extensionName">The name of the extension.</param>
    /// <param name="extension">Returns the extension if found.</param>
    /// <param name="instance">The glTF base instance.</param>
    /// <typeparam name="TExtension">The extension type.</typeparam>
    /// <typeparam name="T">The glTF model type.</typeparam>
    /// <returns>Returns true, if the extension was found.</returns>
    public static bool TryGetExtension<T, TExtension>(this GlTFRef<T> instance, string extensionName, [MaybeNullWhen(false)] out TExtension extension) where T : GlTFProperty
    {
        if (instance.Data.Extensions is null || 
            instance.Data.Extensions.TryGetValue(extensionName, out var extensionObject))
        {
            extension = default;
            return false;
        }

        // The extension is already cased
        if (extensionObject is TExtension extensionType)
        {
            extension = extensionType;
            return true;
        }
        
        // The extension is a JSON object and needs to be deserialized
        if (extensionObject is not JsonObject extensionJsonObject)
        {
            extension = default;
            return false;
        }

        // Deserialize the anonymous object.
        var typeInfo = GlTFSerializerContext.Default.GetTypeInfo(typeof(TExtension));
        if (typeInfo is null || extensionJsonObject.Deserialize(typeInfo) is not TExtension deserializedType)
        {
            extension = default;
            return false;
        }

        extension = deserializedType;
        return true;
    }

    /// <summary>
    /// Sets an extension for a glTF instance.
    /// </summary>
    /// <param name="extensionName">The extension name.</param>
    /// <param name="extension">The extension to set. Set to <c>null</c> to remove the extension.</param>
    /// <param name="instance">The glTF base instance.</param>
    /// <typeparam name="TExtension">The extension type.</typeparam>
    /// <typeparam name="T">The glTF model type.</typeparam>
    public static void SetExtension<T, TExtension>(this GlTFRef<T> instance, string extensionName, TExtension? extension) where T : GlTFProperty
    {
        instance.Data.Extensions ??= new Extension();
        
        if (extension is null)
        {
            instance.Data.Extensions.Remove(extensionName);
        }
        else
        {
            instance.Data.Extensions[extensionName] = extension;
        }
    }
}