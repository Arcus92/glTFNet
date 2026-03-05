namespace glTFNet.Models
{
    /// <summary>
    /// Metadata about the glTF asset.
    /// </summary>
    [System.Serializable]
    public class Asset : GlTFProperty
    {
        /// <summary>
        /// A copyright message suitable for display to credit the content creator.
        /// </summary>
        public System.String? Copyright { get; set; }

        /// <summary>
        /// Tool that generated this glTF model.  Useful for debugging.
        /// </summary>
        public System.String? Generator { get; set; }

        /// <summary>
        /// The glTF version in the form of `&lt;major&gt;.&lt;minor&gt;` that this asset targets.
        /// </summary>
        public required System.String Version { get; set; }

        /// <summary>
        /// The minimum glTF version in the form of `&lt;major&gt;.&lt;minor&gt;` that this asset targets. This property **MUST NOT** be greater than the asset version.
        /// </summary>
        public System.String? MinVersion { get; set; }
    }
}