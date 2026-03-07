namespace glTFNet.Models
{
    /// <summary>
    /// The root object for a glTF asset.
    /// </summary>
    [System.Serializable]
    public class GlTF : glTFNet.Models.GlTFProperty
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
        public glTFNet.Models.Accessor[]? Accessors { get; set; }

        /// <summary>
        /// An array of keyframe animations.
        /// </summary>
        public glTFNet.Models.Animation[]? Animations { get; set; }

        /// <summary>
        /// Metadata about the glTF asset.
        /// </summary>
        public required glTFNet.Models.Asset Asset { get; set; }

        /// <summary>
        /// An array of buffers.  A buffer points to binary geometry, animation, or skins.
        /// </summary>
        public glTFNet.Models.Buffer[]? Buffers { get; set; }

        /// <summary>
        /// An array of bufferViews.  A bufferView is a view into a buffer generally representing a subset of the buffer.
        /// </summary>
        public glTFNet.Models.BufferView[]? BufferViews { get; set; }

        /// <summary>
        /// An array of cameras.  A camera defines a projection matrix.
        /// </summary>
        public glTFNet.Models.Camera[]? Cameras { get; set; }

        /// <summary>
        /// An array of images.  An image defines data used to create a texture.
        /// </summary>
        public glTFNet.Models.Image[]? Images { get; set; }

        /// <summary>
        /// An array of materials.  A material defines the appearance of a primitive.
        /// </summary>
        public glTFNet.Models.Material[]? Materials { get; set; }

        /// <summary>
        /// An array of meshes.  A mesh is a set of primitives to be rendered.
        /// </summary>
        public glTFNet.Models.Mesh[]? Meshes { get; set; }

        /// <summary>
        /// An array of nodes.
        /// </summary>
        public glTFNet.Models.Node[]? Nodes { get; set; }

        /// <summary>
        /// An array of samplers.  A sampler contains properties for texture filtering and wrapping modes.
        /// </summary>
        public glTFNet.Models.Sampler[]? Samplers { get; set; }

        /// <summary>
        /// The index of the default scene.  This property **MUST NOT** be defined, when `scenes` is undefined.
        /// </summary>
        public System.Int32? Scene { get; set; }

        /// <summary>
        /// An array of scenes.
        /// </summary>
        public glTFNet.Models.Scene[]? Scenes { get; set; }

        /// <summary>
        /// An array of skins.  A skin is defined by joints and matrices.
        /// </summary>
        public glTFNet.Models.Skin[]? Skins { get; set; }

        /// <summary>
        /// An array of textures.
        /// </summary>
        public glTFNet.Models.Texture[]? Textures { get; set; }
    }
}