using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.Models;
using JetBrains.Annotations;
using Buffer = glTFNet.Models.Buffer;

namespace glTFNet.Loader;

/// <summary>
/// A wrapper to store the loader reference of a GlTF model.
/// </summary>
/// <param name="model">The data model.</param>
/// <param name="root">The GlTF root model.</param>
/// <param name="loader">The loader this GlTF was loaded from.</param>
/// <typeparam name="T">The model type.</typeparam>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public readonly struct GlTFRef<T>(T model, GlTF root, GlTFLoader loader)
{
    /// <summary>
    /// Gets the underlying model.
    /// </summary>
    public T Model { get; } = model;
    
    /// <summary>
    /// Gets the GlTF root.
    /// </summary>
    public GlTF Root { get; } = root;
    
    /// <summary>
    /// Gets the GlTF loader.
    /// </summary>
    public GlTFLoader Loader { get; } = loader;

    /// <summary>
    /// Creates a new reference.
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="TNew"></typeparam>
    /// <returns></returns>
    public GlTFRef<TNew> Ref<TNew>(TNew instance)
    {
        return new GlTFRef<TNew>(instance, Root, Loader);
    }
    
    /// <summary>
    /// Casts the GlTF reference to the model.
    /// </summary>
    /// <param name="gltfRef">The reference struct.</param>
    /// <returns>Returns the model.</returns>
    public static implicit operator T(GlTFRef<T> gltfRef) => gltfRef.Model;
}

/// <summary>
/// The extension class for <see cref="GlTFRef"/>.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public static class GlTFRef
{
    /// <param name="instance">The GlTF reference.</param>
    extension(GlTFRef<GlTF> instance)
    {
        /// <summary>
        /// Gets the root scene of the GlTF.
        /// </summary>
        /// <value>Returns the scene if found.</value>
        public GlTFRef<Scene>? Scene
        {
            get
            {
                if (!instance.Model.Scene.HasValue || instance.Root.Scenes is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Scenes[instance.Model.Scene.Value]);
            }
        }
    }
    
    /// <param name="instance">The GlTF scene reference.</param>
    extension(GlTFRef<Scene> instance)
    {
        /// <summary>
        /// Gets the root scene of the GlTF.
        /// </summary>
        /// <value>Returns the scene nodes.</value>
        public IEnumerable<GlTFRef<Node>> Nodes
        {
            get
            {
                if (instance.Model.Nodes is null || instance.Root.Nodes is null)
                {
                    return [];
                }

                return instance.Model.Nodes.Select(id => instance.Ref(instance.Root.Nodes[id]));
            }
        }
    }
    
    /// <param name="instance">The GlTF node reference.</param>
    extension(GlTFRef<Node> instance)
    {
        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        /// <value>Returns the child nodes.</value>
        public IEnumerable<GlTFRef<Node>> Children
        {
            get
            {
                if (instance.Model.Children is null || instance.Root.Nodes is null)
                {
                    return [];
                }

                return instance.Model.Children.Select(id => instance.Ref(instance.Root.Nodes[id]));
            }
        }

        /// <summary>
        /// Gets the mesh of this node.
        /// </summary>
        /// <value>Returns the mesh.</value>
        public GlTFRef<Mesh>? Mesh
        {
            get
            {
                if (!instance.Model.Mesh.HasValue || instance.Root.Meshes is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Meshes[instance.Model.Mesh.Value]);
            }
        }

        /// <summary>
        /// Gets the camera of this node.
        /// </summary>
        /// <value>Returns the camera.</value>
        public GlTFRef<Camera>? Camera
        {
            get
            {
                if (!instance.Model.Camera.HasValue || instance.Root.Cameras is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Cameras[instance.Model.Camera.Value]);
            }
        }
    }

    /// <param name="instance">The GlTF mesh reference.</param>
    extension(GlTFRef<Mesh> instance)
    {
        /// <summary>
        /// Gets the primitives of this mesh.
        /// </summary>
        /// <value>Returns the primitives.</value>
        public IEnumerable<GlTFRef<MeshPrimitive>> Primitives => instance.Model.Primitives.Select(instance.Ref);
    }
    
    /// <param name="instance">The GlTF mesh primitive reference.</param>
    extension(GlTFRef<MeshPrimitive> instance)
    {
        /// <summary>
        /// Gets the buffer accessor for the indices of this mesh.
        /// </summary>
        /// <value>Returns the indices accessor.</value>
        public GlTFRef<Accessor>? Indices
        {
            get
            {
                if (!instance.Model.Indices.HasValue || instance.Root.Accessors is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Accessors[instance.Model.Indices.Value]);
            }
        }

        /// <summary>
        /// Gets all attributes as name/accessor pair.
        /// </summary>
        public IEnumerable<KeyValuePair<string, GlTFRef<Accessor>>> Attributes
        {
            get
            {
                if (instance.Root.Accessors is null)
                {
                    return [];
                }
                
                return instance.Model.Attributes.Select(kvp => new KeyValuePair<string, GlTFRef<Accessor>>(
                    kvp.Key, 
                    instance.Ref(instance.Root.Accessors[kvp.Value])
                    ));
            }
        }
        
        /// <summary>
        /// Tries to get the attribute accessor by its name.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="accessor">Returns the accessor if found.</param>
        /// <returns>Returns true, if the attribute was found.</returns>
        public bool TryGetAttribute(string name, out GlTFRef<Accessor> accessor)
        {
            accessor = default;
            if (instance.Root.Accessors is null)
            {
                return false;
            }

            if (!instance.Model.Attributes.TryGetValue(name, out var index))
            {
                return false;
            }

            accessor = instance.Ref(instance.Root.Accessors[index]);
            return true;
        }

        /// <summary>
        /// Gets the material of this mesh.
        /// </summary>
        /// <value>Returns the material.</value>
        public GlTFRef<Material>? Material
        {
            get
            {
                if (!instance.Model.Material.HasValue || instance.Root.Materials is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Materials[instance.Model.Material.Value]);
            }
        }
    }

    /// <param name="instance">The GlTF accessor reference.</param>
    extension(GlTFRef<Accessor> instance)
    {
        /// <summary>
        /// Gets the buffer view of this accessor.
        /// </summary>
        /// <value>Returns the buffer view.</value>
        public GlTFRef<BufferView>? BufferView
        {
            get
            {
                if (!instance.Model.BufferView.HasValue || instance.Root.BufferViews is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.BufferViews[instance.Model.BufferView.Value]);
            }
        }

        /// <summary>
        /// Reads the data from this accessor.
        /// </summary>
        /// <returns>Returns the data.</returns>
        public async Task<Array?> Read()
        {
            if (!instance.BufferView.HasValue)
            {
                return null;
            }

            // Loads the buffer view
            var bufferView = await instance.BufferView.Value.Load();
            return bufferView?.Read(instance.Model);
        }
        
        /// <summary>
        /// Reads the data from this accessor.
        /// </summary>
        /// <returns>Returns the data.</returns>
        public async Task<T[]?> Read<T>() where T : struct
        {
            if (!instance.BufferView.HasValue)
            {
                return null;
            }

            // Loads the buffer view
            var bufferView = await instance.BufferView.Value.Load();
            return bufferView?.Read<T>(instance.Model);
        }
    }
    
    /// <param name="instance">The GlTF buffer view reference.</param>
    extension(GlTFRef<BufferView> instance)
    {
        /// <summary>
        /// Gets the buffer of this buffer view.
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

                return instance.Ref(instance.Root.Buffers[instance.Model.Buffer]);
            }
        }

        /// <summary>
        /// Opens the buffer view.
        /// </summary>
        /// <returns>Returns the buffer view.</returns>
        public async Task<GlTFBufferView?> Load()
        {
            var buffer = instance.Buffer?.Load();
            if (buffer is null)
            {
                return null;
            }

            return await buffer.OpenBufferView(instance.Model);
        }
    }

    /// <param name="instance">The GlTF buffer reference.</param>r
    extension(GlTFRef<Buffer> instance)
    {
        /// <summary>
        /// Opens the buffer.
        /// </summary>
        /// <returns>Returns the buffer.</returns>
        public GlTFBuffer Load()
        {
            if (!instance.Loader.TryResolveBuffer(instance.Model.Uri, out var buffer))
            {
                throw new Exception($"Could not resolve buffer: {instance.Model.Uri ?? "(null)"}");
            }

            return buffer;
        }
    }
    

    /// <param name="instance">The GlTF material reference.</param>
    extension(GlTFRef<Material> instance)
    {
        /// <summary>
        /// Gets the pbr material properties.
        /// </summary>
        /// <value>Returns the pbr material properties.</value>
        public GlTFRef<MaterialPbrMetallicRoughness>? PbrMetallicRoughness
        {
            get
            {
                if (instance.Model.PbrMetallicRoughness is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.PbrMetallicRoughness);
            }
        }

        /// <summary>
        /// Gets the normal texture info.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<MaterialNormalTextureInfo>? NormalTexture
        {
            get
            {
                if (instance.Model.NormalTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.NormalTexture);
            }
        }

        /// <summary>
        /// Gets the occlusion texture info.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<MaterialOcclusionTextureInfo>? OcclusionTexture
        {
            get
            {
                if (instance.Model.OcclusionTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.OcclusionTexture);
            }
        }

        /// <summary>
        /// Gets the emissive texture info.
        /// </summary>
        /// <value>Returns the texture info.</value>
        public GlTFRef<TextureInfo>? EmissiveTexture
        {
            get
            {
                if (instance.Model.EmissiveTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.EmissiveTexture);
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
                if (instance.Model.BaseColorTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.BaseColorTexture);
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
                if (instance.Model.MetallicRoughnessTexture is null)
                {
                    return null;
                }

                return instance.Ref(instance.Model.MetallicRoughnessTexture);
            }
        }
    }

    /// <param name="instance">The GlTF texture info reference.</param>
    extension(GlTFRef<TextureInfo> instance)
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

                return instance.Ref(instance.Root.Textures[instance.Model.Index]);
            }
        }
    }
    
    /// <param name="instance">The GlTF texture reference.</param>
    extension(GlTFRef<Texture> instance)
    {
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>Returns the source image.</value>
        public GlTFRef<Image>? Source
        {
            get
            {
                if (!instance.Model.Source.HasValue || instance.Root.Images is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Images[instance.Model.Source.Value]);
            }
        }
        
        /// <summary>
        /// Gets the texture sampler.
        /// </summary>
        /// <value>Returns the sampler.</value>
        public GlTFRef<Sampler>? Sampler
        {
            get
            {
                if (!instance.Model.Sampler.HasValue || instance.Root.Samplers is null)
                {
                    return null;
                }

                return instance.Ref(instance.Root.Samplers[instance.Model.Sampler.Value]);
            }
        }
    }

    /// <param name="instance">The GlTF image reference.</param>
    extension(GlTFRef<Image> instance)
    {
        /// <summary>
        /// Opens the image.
        /// </summary>
        /// <returns>Returns the image stream.</returns>
        public Stream Load()
        {
            if (!instance.Loader.TryResolveStream(instance.Model.Uri, out var stream))
            {
                throw new Exception($"Could not resolve image: {instance.Model.Uri ?? "(null)"}");
            }

            return stream;
        }
    }

    /// <param name="instance">The GlTF base instance.</param>
    extension(GlTFRef<GlTFProperty> instance)
    {
        /// <summary>
        /// Tries to get an extension object by the given name.
        /// </summary>
        /// <param name="extensionName">The name of the extension.</param>
        /// <param name="extension">Returns the extension if found.</param>
        /// <typeparam name="T">The extension type.</typeparam>
        /// <returns>Returns true, if the extension was found.</returns>
        public bool TryGetExtension<T>(string extensionName, [MaybeNullWhen(false)] out T extension)
        {
            if (instance.Model.Extensions is null || 
                instance.Model.Extensions.TryGetValue(extensionName, out var extensionObject))
            {
                extension = default;
                return false;
            }

            // The extension is already cased
            if (extensionObject is T extensionType)
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
            var typeInfo = GlTFSerializerContext.Default.GetTypeInfo(typeof(T));
            if (typeInfo is null || extensionJsonObject.Deserialize(typeInfo) is not T deserializedType)
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
        /// <typeparam name="T">The extension type.</typeparam>
        public void SetExtension<T>(string extensionName, T? extension)
        {
            instance.Model.Extensions ??= new Extension();
        
            if (extension is null)
            {
                instance.Model.Extensions.Remove(extensionName);
            }
            else
            {
                instance.Model.Extensions[extensionName] = extension;
            }
        }
    }
}