namespace glTFNet.Models
{
    /// <summary>
    /// Sparse storage of accessor values that deviate from their initialization value.
    /// </summary>
    [System.Serializable]
    public class AccessorSparse : glTFNet.Models.GlTFProperty
    {
        /// <summary>
        /// Number of deviating accessor values stored in the sparse array.
        /// </summary>
        public required System.Int32 Count { get; set; }

        /// <summary>
        /// An object pointing to a buffer view containing the indices of deviating accessor values. The number of indices is equal to `count`. Indices **MUST** strictly increase.
        /// </summary>
        public required glTFNet.Models.AccessorSparseIndices Indices { get; set; }

        /// <summary>
        /// An object pointing to a buffer view containing the deviating accessor values.
        /// </summary>
        public required glTFNet.Models.AccessorSparseValues Values { get; set; }
    }
}