namespace glTFNet.Models
{
    /// <summary>
    /// A buffer points to binary geometry, animation, or skins.
    /// </summary>
    [System.Serializable]
    public class Buffer : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// The URI (or IRI) of the buffer.  Relative paths are relative to the current glTF asset.  Instead of referencing an external file, this field **MAY** contain a `data:`-URI.
        /// </summary>
        public System.String? Uri { get; set; }

        /// <summary>
        /// The length of the buffer in bytes.
        /// </summary>
        public required System.Int32 ByteLength { get; set; }
    }
}