namespace glTFNet.Models
{
    /// <summary>
    /// Texture sampler properties for filtering and wrapping modes.
    /// </summary>
    [System.Serializable]
    public class Sampler : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// Magnification filter.
        /// </summary>
        public glTFNet.Models.SamplerMagFilter? MagFilter { get; set; }

        /// <summary>
        /// Minification filter.
        /// </summary>
        public glTFNet.Models.SamplerMinFilter? MinFilter { get; set; }

        /// <summary>
        /// S (U) wrapping mode.  All valid values correspond to WebGL enums.
        /// </summary>
        public glTFNet.Models.SamplerWrapS? WrapS { get; set; }

        /// <inheritdoc cref="WrapS"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public glTFNet.Models.SamplerWrapS WrapSOrDefault => WrapS ?? glTFNet.Models.SamplerWrapS.Repeat;

        /// <summary>
        /// T (V) wrapping mode.
        /// </summary>
        public glTFNet.Models.SamplerWrapT? WrapT { get; set; }

        /// <inheritdoc cref="WrapT"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public glTFNet.Models.SamplerWrapT WrapTOrDefault => WrapT ?? glTFNet.Models.SamplerWrapT.Repeat;
    }
}