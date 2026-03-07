namespace glTFNet.Models
{
    public enum MaterialAlphaMode
    {
        /// <summary>
        /// The alpha value is ignored, and the rendered output is fully opaque.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("OPAQUE")]
        Opaque = 0,
        /// <summary>
        /// The rendered output is either fully opaque or fully transparent depending on the alpha value and the specified `alphaCutoff` value; the exact appearance of the edges **MAY** be subject to implementation-specific techniques such as "`Alpha-to-Coverage`".
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("MASK")]
        Mask = 0,
        /// <summary>
        /// The alpha value is used to composite the source and destination areas. The rendered output is combined with the background using the normal painting operation (i.e. the Porter and Duff over operator).
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("BLEND")]
        Blend = 0
    }
}