namespace glTFNet.Models
{
    /// <summary>
    /// An object pointing to a buffer view containing the indices of deviating accessor values. The number of indices is equal to `accessor.sparse.count`. Indices **MUST** strictly increase.
    /// </summary>
    [System.Serializable]
    public class AccessorSparseIndices : glTFNet.Models.GlTFProperty
    {
        /// <summary>
        /// The index of the buffer view with sparse indices. The referenced buffer view **MUST NOT** have its `target` or `byteStride` properties defined. The buffer view and the optional `byteOffset` **MUST** be aligned to the `componentType` byte length.
        /// </summary>
        public required System.Int32 BufferView { get; set; }

        /// <summary>
        /// The offset relative to the start of the buffer view in bytes.
        /// </summary>
        public System.Int32? ByteOffset { get; set; }

        /// <inheritdoc cref="ByteOffset"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Int32 ByteOffsetOrDefault => ByteOffset ?? 0;

        /// <summary>
        /// The indices data type.
        /// </summary>
        public required glTFNet.Models.AccessorSparseIndicesComponentType ComponentType { get; set; }
    }
}