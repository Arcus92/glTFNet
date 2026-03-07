namespace glTFNet.Models
{
    /// <summary>
    /// An object pointing to a buffer view containing the deviating accessor values. The number of elements is equal to `accessor.sparse.count` times number of components. The elements have the same component type as the base accessor. The elements are tightly packed. Data **MUST** be aligned following the same rules as the base accessor.
    /// </summary>
    [System.Serializable]
    public class AccessorSparseValues : glTFNet.Models.GlTFProperty
    {
        /// <summary>
        /// The index of the bufferView with sparse values. The referenced buffer view **MUST NOT** have its `target` or `byteStride` properties defined.
        /// </summary>
        public required System.Int32 BufferView { get; set; }

        /// <summary>
        /// The offset relative to the start of the bufferView in bytes.
        /// </summary>
        public System.Int32? ByteOffset { get; set; }

        /// <inheritdoc cref="ByteOffset"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Int32 ByteOffsetOrDefault => ByteOffset ?? 0;
    }
}