namespace glTFNet.Models
{
    /// <summary>
    /// Texture sampler properties for filtering and wrapping modes.
    /// </summary>
    [System.Serializable]
    public class Sampler : GlTFChildOfRootProperty
    {
        /// <summary>
        /// Magnification filter.
        /// </summary>
        public System.Int32? MagFilter { get; set; }

        /// <summary>
        /// Minification filter.
        /// </summary>
        public System.Int32? MinFilter { get; set; }

        /// <summary>
        /// S (U) wrapping mode.  All valid values correspond to WebGL enums.
        /// </summary>
        public System.Int32? WrapS { get; set; }

        /// <inheritdoc cref="WrapS"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Int32 WrapSOrDefault => WrapS ?? 10497;

        /// <summary>
        /// T (V) wrapping mode.
        /// </summary>
        public System.Int32? WrapT { get; set; }

        /// <inheritdoc cref="WrapT"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Int32 WrapTOrDefault => WrapT ?? 10497;
    }
}