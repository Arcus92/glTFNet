namespace glTFNet.Models
{
    /// <summary>
    /// Joints and matrices defining a skin.
    /// </summary>
    [System.Serializable]
    public class Skin : GlTFChildOfRootProperty
    {
        /// <summary>
        /// The index of the accessor containing the floating-point 4x4 inverse-bind matrices. Its `accessor.count` property **MUST** be greater than or equal to the number of elements of the `joints` array. When undefined, each matrix is a 4x4 identity matrix.
        /// </summary>
        public System.Int32? InverseBindMatrices { get; set; }

        /// <summary>
        /// The index of the node used as a skeleton root. The node **MUST** be the closest common root of the joints hierarchy or a direct or indirect parent node of the closest common root.
        /// </summary>
        public System.Int32? Skeleton { get; set; }

        /// <summary>
        /// Indices of skeleton nodes, used as joints in this skin.
        /// </summary>
        public required System.Int32[] Joints { get; set; }
    }
}