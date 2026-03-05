namespace glTFNet.Models
{
    /// <summary>
    /// The root object for a glTF asset.
    /// </summary>
    [System.Serializable]
    public class GlTF : GlTFProperty
    {
        /// <summary>
        /// Names of glTF extensions used in this asset.
        /// </summary>
        public System.String[]? ExtensionsUsed { get; set; }

        /// <summary>
        /// Names of glTF extensions required to properly load this asset.
        /// </summary>
        public System.String[]? ExtensionsRequired { get; set; }

        /// <summary>
        /// An array of accessors.  An accessor is a typed view into a bufferView.
        /// </summary>
        public Accessor[]? Accessors { get; set; }

        /// <summary>
        /// An array of keyframe animations.
        /// </summary>
        public Animation[]? Animations { get; set; }

        /// <summary>
        /// Metadata about the glTF asset.
        /// </summary>
        public required Asset Asset { get; set; }

        /// <summary>
        /// An array of buffers.  A buffer points to binary geometry, animation, or skins.
        /// </summary>
        public Buffer[]? Buffers { get; set; }

        /// <summary>
        /// An array of bufferViews.  A bufferView is a view into a buffer generally representing a subset of the buffer.
        /// </summary>
        public BufferView[]? BufferViews { get; set; }

        /// <summary>
        /// An array of cameras.  A camera defines a projection matrix.
        /// </summary>
        public Camera[]? Cameras { get; set; }

        /// <summary>
        /// An array of images.  An image defines data used to create a texture.
        /// </summary>
        public Image[]? Images { get; set; }

        /// <summary>
        /// An array of materials.  A material defines the appearance of a primitive.
        /// </summary>
        public Material[]? Materials { get; set; }

        /// <summary>
        /// An array of meshes.  A mesh is a set of primitives to be rendered.
        /// </summary>
        public Mesh[]? Meshes { get; set; }

        /// <summary>
        /// An array of nodes.
        /// </summary>
        public Node[]? Nodes { get; set; }

        /// <summary>
        /// An array of samplers.  A sampler contains properties for texture filtering and wrapping modes.
        /// </summary>
        public Sampler[]? Samplers { get; set; }

        /// <summary>
        /// The index of the default scene.  This property **MUST NOT** be defined, when `scenes` is undefined.
        /// </summary>
        public System.Int32? Scene { get; set; }

        /// <summary>
        /// An array of scenes.
        /// </summary>
        public Scene[]? Scenes { get; set; }

        /// <summary>
        /// An array of skins.  A skin is defined by joints and matrices.
        /// </summary>
        public Skin[]? Skins { get; set; }

        /// <summary>
        /// An array of textures.
        /// </summary>
        public Texture[]? Textures { get; set; }
    }
}