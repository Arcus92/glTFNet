namespace glTFNet.Models
{
    /// <summary>
    /// Geometry to be rendered with the given material.
    /// </summary>
    [System.Serializable]
    public class MeshPrimitive : GlTFProperty
    {
        /// <summary>
        /// A plain JSON object, where each key corresponds to a mesh attribute semantic and each value is the index of the accessor containing attribute's data.
        /// </summary>
        public required System.Object Attributes { get; set; }

        /// <summary>
        /// The index of the accessor that contains the vertex indices.  When this is undefined, the primitive defines non-indexed geometry.  When defined, the accessor **MUST** have `SCALAR` type and an unsigned integer component type.
        /// </summary>
        public System.Int32? Indices { get; set; }

        /// <summary>
        /// The index of the material to apply to this primitive when rendering.
        /// </summary>
        public System.Int32? Material { get; set; }

        /// <summary>
        /// The topology type of primitives to render.
        /// </summary>
        public System.Int32? Mode { get; set; }

        /// <inheritdoc cref="Mode"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Int32 ModeOrDefault => Mode ?? 4;

        /// <summary>
        /// An array of morph targets.
        /// </summary>
        public System.Object[]? Targets { get; set; }
    }
}