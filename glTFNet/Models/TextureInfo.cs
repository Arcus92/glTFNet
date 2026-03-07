namespace glTFNet.Models
{
    /// <summary>
    /// Reference to a texture.
    /// </summary>
    [Serializable]
    public class TextureInfo : GlTFProperty
    {
        /// <summary>
        /// The index of the texture.
        /// </summary>
        public required int Index { get; set; }

        /// <summary>
        /// This integer value is used to construct a string in the format `TEXCOORD_&lt;set index&gt;` which is a reference to a key in `mesh.primitives.attributes` (e.g. a value of `0` corresponds to `TEXCOORD_0`). A mesh primitive **MUST** have the corresponding texture coordinate attributes for the material to be applicable to it.
        /// </summary>
        public int? TexCoord { get; set; }

        /// <inheritdoc cref="TexCoord"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public int TexCoordOrDefault => TexCoord ?? 0;
    }
}