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
    /// <param name="instance">The GlTF reference.</param>
    extension<T>(GlTFRef<T>? instance)
    {
        /// <inheritdoc cref="GlTFRef{T}.Data" />
        public T? Data => instance.HasValue ? instance.Value.Data : default;
        
        /// <summary>
        /// Tries to unwrap the nullable glTF reference.
        /// </summary>
        /// <param name="result">Returns the unwrapped reference.</param>
        /// <returns>Returns true, if this nullable wrapper has a value.</returns>
        public bool TryGet(out GlTFRef<T> result)
        {
            if (!instance.HasValue)
            {
                result = default;
                return false;
            }

            result = instance.Value;
            return true;
        }
    }

    /// <param name="instance">The GlTF reference.</param>
    extension(GlTFRef<GlTF> instance)
    {
        /// <summary>
        /// Gets the asset of the glTF root.
        /// </summary>
        public GlTFRef<Asset>? Asset => instance.Ref(instance.Data.Asset);

        /// <summary>
        /// Gets the root scene of the glTF root.
        /// </summary>
        /// <value>Returns the scene if found.</value>
        public GlTFRef<Scene>? Scene
        {
            get
            {
                if (!instance.Data.Scene.HasValue || instance.Root.Scenes is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Scenes, instance.Data.Scene.Value);
            }
        }

        /// <summary>
        /// Gets all scenes of the glTF root.
        /// </summary>
        public GlTFListRef<Scene> Scenes => instance.Root.Scenes is null ? GlTFListRef<Scene>.Empty : instance.RefList(instance.Root.Scenes);
        
        /// <summary>
        /// Gets all nodes of the glTF root.
        /// </summary>
        public GlTFListRef<Node> Nodes => instance.Root.Nodes is null ? GlTFListRef<Node>.Empty : instance.RefList(instance.Root.Nodes);
        
        /// <summary>
        /// Gets all cameras of the glTF root.
        /// </summary>
        public GlTFListRef<Camera> Cameras => instance.Root.Cameras is null ? GlTFListRef<Camera>.Empty : instance.RefList(instance.Root.Cameras);
        
        /// <summary>
        /// Gets all materials of the glTF root.
        /// </summary>
        public GlTFListRef<Material> Materials => instance.Root.Materials is null ? GlTFListRef<Material>.Empty : instance.RefList(instance.Root.Materials);
        
        /// <summary>
        /// Gets all textures of the glTF root.
        /// </summary>
        public GlTFListRef<Texture> Textures => instance.Root.Textures is null ? GlTFListRef<Texture>.Empty : instance.RefList(instance.Root.Textures);
        
        /// <summary>
        /// Gets all samplers of the glTF root.
        /// </summary>
        public GlTFListRef<Sampler> Samplers => instance.Root.Samplers is null ? GlTFListRef<Sampler>.Empty : instance.RefList(instance.Root.Samplers);
        
        /// <summary>
        /// Gets all images of the glTF root.
        /// </summary>
        public GlTFListRef<Image> Images => instance.Root.Images is null ? GlTFListRef<Image>.Empty : instance.RefList(instance.Root.Images);
        
        /// <summary>
        /// Gets all buffers of the glTF root.
        /// </summary>
        public GlTFListRef<Buffer> Buffers => instance.Root.Buffers is null ? GlTFListRef<Buffer>.Empty : instance.RefList(instance.Root.Buffers);
        
        /// <summary>
        /// Gets all buffer views of the glTF root.
        /// </summary>
        public GlTFListRef<BufferView> BufferViews => instance.Root.BufferViews is null ? GlTFListRef<BufferView>.Empty : instance.RefList(instance.Root.BufferViews);
        
        /// <summary>
        /// Gets all accessors of the glTF root.
        /// </summary>
        public GlTFListRef<Accessor> Accessors => instance.Root.Accessors is null ? GlTFListRef<Accessor>.Empty : instance.RefList(instance.Root.Accessors);
        
        /// <summary>
        /// Gets all meshes of the glTF root.
        /// </summary>
        public GlTFListRef<Mesh> Meshes => instance.Root.Meshes is null ? GlTFListRef<Mesh>.Empty : instance.RefList(instance.Root.Meshes);
        
        /// <summary>
        /// Gets all skins of the glTF root.
        /// </summary>
        public GlTFListRef<Skin> Skins => instance.Root.Skins is null ? GlTFListRef<Skin>.Empty : instance.RefList(instance.Root.Skins);
        
        /// <summary>
        /// Gets all animations of the glTF root.
        /// </summary>
        public GlTFListRef<Animation> Animations => instance.Root.Animations is null ? GlTFListRef<Animation>.Empty : instance.RefList(instance.Root.Animations);
    }
    
    /// <param name="instance">The GlTF scene reference.</param>
    extension(GlTFRef<Scene> instance)
    {
        /// <summary>
        /// Gets the root scene of the glTF scene.
        /// </summary>
        /// <value>Returns the scene nodes.</value>
        public GlTFIndexedListRef<Node> Nodes
        {
            get
            {
                if (instance.Data.Nodes is null || instance.Root.Nodes is null)
                {
                    return GlTFIndexedListRef<Node>.Empty;
                }

                return instance.RefIndexedList(instance.Root.Nodes, instance.Data.Nodes);
            }
        }
    }
    
    /// <param name="instance">The GlTF node reference.</param>
    extension(GlTFRef<Node> instance)
    {
        /// <summary>
        /// Gets the children of the glTF node.
        /// </summary>
        /// <value>Returns the child nodes.</value>
        public GlTFIndexedListRef<Node> Children
        {
            get
            {
                if (instance.Data.Children is null || instance.Root.Nodes is null)
                {
                    return GlTFIndexedListRef<Node>.Empty;
                }

                return instance.RefIndexedList(instance.Root.Nodes, instance.Data.Children);
            }
        }

        /// <summary>
        /// Gets the mesh of the glTF node.
        /// </summary>
        /// <value>Returns the mesh.</value>
        public GlTFRef<Mesh>? Mesh
        {
            get
            {
                if (!instance.Data.Mesh.HasValue || instance.Root.Meshes is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Meshes, instance.Data.Mesh.Value);
            }
        }

        /// <summary>
        /// Gets the camera of the glTF node.
        /// </summary>
        /// <value>Returns the camera.</value>
        public GlTFRef<Camera>? Camera
        {
            get
            {
                if (!instance.Data.Camera.HasValue || instance.Root.Cameras is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Cameras, instance.Data.Camera.Value);
            }
        }
    }

    /// <param name="instance">The GlTF mesh reference.</param>
    extension(GlTFRef<Mesh> instance)
    {
        /// <summary>
        /// Gets the primitives of the glTF mesh.
        /// </summary>
        /// <value>Returns the primitives.</value>
        public GlTFListRef<MeshPrimitive> Primitives => instance.RefList(instance.Data.Primitives);
    }
    
    /// <param name="instance">The GlTF mesh primitive reference.</param>
    extension(GlTFRef<MeshPrimitive> instance)
    {
        /// <summary>
        /// Gets the buffer accessor for the indices of the glTF mesh.
        /// </summary>
        /// <value>Returns the indices accessor.</value>
        public GlTFRef<Accessor>? Indices
        {
            get
            {
                if (!instance.Data.Indices.HasValue || instance.Root.Accessors is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Accessors, instance.Data.Indices.Value);
            }
        }

        /// <summary>
        /// Gets all attributes as name/accessor pair of the glTF mesh.
        /// </summary>
        public GlTFIndexedDictionaryRef<string, Accessor> Attributes
        {
            get
            {
                if (instance.Root.Accessors is null)
                {
                    return GlTFIndexedDictionaryRef<string, Accessor>.Empty;
                }

                return instance.RefIndexedDictionary(instance.Root.Accessors, instance.Data.Attributes);
            }
        }

        /// <summary>
        /// Gets the material of the glTF mesh.
        /// </summary>
        /// <value>Returns the material.</value>
        public GlTFRef<Material>? Material
        {
            get
            {
                if (!instance.Data.Material.HasValue || instance.Root.Materials is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Materials, instance.Data.Material.Value);
            }
        }
    }

    /// <param name="instance">The GlTF accessor reference.</param>
    extension(GlTFRef<Accessor> instance)
    {
        /// <summary>
        /// Gets the buffer view of the glTF accessor.
        /// </summary>
        /// <value>Returns the buffer view.</value>
        public GlTFRef<BufferView>? BufferView
        {
            get
            {
                if (!instance.Data.BufferView.HasValue || instance.Root.BufferViews is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.BufferViews, instance.Data.BufferView.Value);
            }
        }

        /// <summary>
        /// Reads the data from the glTF accessor.
        /// </summary>
        /// <remarks>
        /// Only Scalar is fully supported! Vec2, Vec3, Vec4 and Mat4x4 only support float values. Mat2x2 and Mat3x3
        /// are not supported.<br/>
        /// You can use <see cref="Read{T}"/> to pass a custom struct with the given length to bypass this limitation.
        /// </remarks>
        /// <returns>Returns the data.</returns>
        public async Task<Array?> Read()
        {
            if (!instance.BufferView.HasValue)
            {
                return null;
            }

            // Loads the buffer view
            var bufferView = await instance.BufferView.Value.Open();
            return bufferView?.Read(instance.Data);
        }
        
        /// <summary>
        /// Reads the data from the glTF accessor as the given type.
        /// </summary>
        /// <remarks>
        /// The component type must match the component size in the accessor.
        /// </remarks>
        /// <typeparam name="T">The component type to read.</typeparam>
        /// <returns>Returns the data.</returns>
        public async Task<T[]?> Read<T>() where T : struct
        {
            if (!instance.BufferView.HasValue)
            {
                return null;
            }

            // Loads the buffer view
            var bufferView = await instance.BufferView.Value.Open();
            return bufferView?.Read<T>(instance.Data);
        }
    }
    
    /// <param name="instance">The GlTF buffer view reference.</param>
    extension(GlTFRef<BufferView> instance)
    {
        /// <summary>
        /// Gets the buffer of the glTF buffer view.
        /// </summary>
        /// <value>Returns the buffer.</value>
        public GlTFRef<Buffer>? Buffer
        {
            get
            {
                if (instance.Root.Buffers is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Buffers, instance.Data.Buffer);
            }
        }

        /// <summary>
        /// Opens the glTF buffer view.
        /// </summary>
        /// <returns>Returns the buffer view.</returns>
        public async Task<GlTFBufferView?> Open()
        {
            if (!instance.Buffer.HasValue)
            {
                return null;
            }

            var buffer = await instance.Buffer.Value.Open();
            return await buffer.OpenBufferView(instance.Data);
        }
    }

    /// <param name="instance">The GlTF buffer reference.</param>r
    extension(GlTFRef<Buffer> instance)
    {
        /// <summary>
        /// Opens the buffer.
        /// </summary>
        /// <returns>Returns the buffer.</returns>
        public async Task<GlTFBuffer> Open()
        {
            var buffer = await instance.Loader.OpenUriAsBuffer(instance.Data.Uri);
            if (buffer is null)
            {
                throw new Exception($"Could not resolve buffer: {instance.Data.Uri ?? "(null)"}");
            }

            return buffer;
        }
    }
    

    /// <param name="instance">The GlTF material reference.</param>
    extension(GlTFRef<Material> instance)
    {
        /// <summary>
        /// Gets the pbr material properties of the glTF material.
        /// </summary>
        /// <value>Returns the pbr material properties.</value>
        public GlTFRef<MaterialPbrMetallicRoughness>? PbrMetallicRoughness
        {
            get
            {
                if (instance.Data.PbrMetallicRoughness is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.PbrMetallicRoughness);
            }
        }

        /// <summary>
        /// Gets the normal texture info of the glTF material.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<MaterialNormalTextureInfo>? NormalTexture
        {
            get
            {
                if (instance.Data.NormalTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.NormalTexture);
            }
        }

        /// <summary>
        /// Gets the occlusion texture info of the glTF material.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<MaterialOcclusionTextureInfo>? OcclusionTexture
        {
            get
            {
                if (instance.Data.OcclusionTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.OcclusionTexture);
            }
        }

        /// <summary>
        /// Gets the emissive texture info of the glTF material.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<TextureInfo>? EmissiveTexture
        {
            get
            {
                if (instance.Data.EmissiveTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.EmissiveTexture);
            }
        }
    }

    /// <param name="instance">The GlTF pbr material reference.</param>
    extension(GlTFRef<MaterialPbrMetallicRoughness> instance)
    {
        /// <summary>
        /// Gets the base color texture info.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<TextureInfo>? BaseColorTexture
        {
            get
            {
                if (instance.Data.BaseColorTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.BaseColorTexture);
            }
        }

        /// <summary>
        /// Gets the metallic roughness texture info.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<TextureInfo>? MetallicRoughnessTexture
        {
            get
            {
                if (instance.Data.MetallicRoughnessTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Data.MetallicRoughnessTexture);
            }
        }
    }

    /// <param name="instance">The GlTF texture info reference.</param>
    extension<T>(GlTFRef<T> instance) where T : TextureInfo
    {
        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>Returns the texture.</value>
        public GlTFRef<Texture>? Texture
        {
            get
            {
                if (instance.Root.Textures is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Textures, instance.Data.Index);
            }
        }
    }
    
    /// <param name="instance">The GlTF texture reference.</param>
    extension(GlTFRef<Texture> instance)
    {
        /// <summary>
        /// Gets the image of the glTF image.
        /// </summary>
        /// <value>Returns the source image.</value>
        public GlTFRef<Image>? Source
        {
            get
            {
                if (!instance.Data.Source.HasValue || instance.Root.Images is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Images, instance.Data.Source.Value);
            }
        }
        
        /// <summary>
        /// Gets the texture sampler of the glTF image.
        /// </summary>
        /// <value>Returns the sampler.</value>
        public GlTFRef<Sampler>? Sampler
        {
            get
            {
                if (!instance.Data.Sampler.HasValue || instance.Root.Samplers is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Samplers, instance.Data.Sampler.Value);
            }
        }
    }

    /// <param name="instance">The GlTF image reference.</param>
    extension(GlTFRef<Image> instance)
    {
        /// <summary>
        /// Gets the buffer view of the glTF image.
        /// </summary>
        /// <value>Returns the buffer view.</value>
        public GlTFRef<BufferView>? BufferView
        {
            get
            {
                if (!instance.Data.BufferView.HasValue || instance.Root.BufferViews is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.BufferViews, instance.Data.BufferView.Value);
            }
        }
        
        /// <summary>
        /// Opens the image.
        /// </summary>
        /// <returns>Returns the image stream.</returns>
        public async Task<Stream> Open()
        {
            // Load from buffer
            if (instance.BufferView.TryGet(out var bufferView))
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
    }

    /// <param name="instance">The GlTF base instance.</param>
    extension<T>(GlTFRef<T> instance) where T : GlTFProperty
    {
        /// <summary>
        /// Tries to get an extension object by the given name.
        /// </summary>
        /// <param name="extensionName">The name of the extension.</param>
        /// <param name="extension">Returns the extension if found.</param>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        /// <returns>Returns true, if the extension was found.</returns>
        public bool TryGetExtension<TExtension>(string extensionName, [MaybeNullWhen(false)] out TExtension extension)
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
        /// Sets an extension for a GlTF instance.
        /// </summary>
        /// <param name="extensionName">The extension name.</param>
        /// <param name="extension">The extension to set. Set to <c>null</c> to remove the extension.</param>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        public void SetExtension<TExtension>(string extensionName, TExtension? extension)
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
}