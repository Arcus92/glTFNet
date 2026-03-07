namespace glTFNet.Models
{
    /// <summary>
    /// A set of primitives to be rendered.  Its global transform is defined by a node that references it.
    /// </summary>
    [System.Serializable]
    public class Mesh : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// An array of primitives, each defining geometry to be rendered.
        /// </summary>
        public required glTFNet.Models.MeshPrimitive[] Primitives { get; set; }

        /// <summary>
        /// Array of weights to be applied to the morph targets. The number of array elements **MUST** match the number of morph targets.
        /// </summary>
        public System.Single[]? Weights { get; set; }
    }
}