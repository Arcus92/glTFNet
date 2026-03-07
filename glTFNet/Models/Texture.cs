namespace glTFNet.Models
{
    /// <summary>
    /// A texture and its sampler.
    /// </summary>
    [System.Serializable]
    public class Texture : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// The index of the sampler used by this texture. When undefined, a sampler with repeat wrapping and auto filtering **SHOULD** be used.
        /// </summary>
        public System.Int32? Sampler { get; set; }

        /// <summary>
        /// The index of the image used by this texture. When undefined, an extension or other mechanism **SHOULD** supply an alternate texture source, otherwise behavior is undefined.
        /// </summary>
        public System.Int32? Source { get; set; }
    }
}