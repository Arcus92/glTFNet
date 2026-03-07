namespace glTFNet.Models
{
    [System.Serializable]
    public class MaterialNormalTextureInfo : glTFNet.Models.TextureInfo
    {
        /// <summary>
        /// The scalar parameter applied to each normal vector of the texture. This value scales the normal vector in X and Y directions using the formula: `scaledNormal =  normalize((&lt;sampled normal texture value&gt; * 2.0 - 1.0) * vec3(&lt;normal scale&gt;, &lt;normal scale&gt;, 1.0))`.
        /// </summary>
        public System.Single? Scale { get; set; }

        /// <inheritdoc cref="Scale"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Single ScaleOrDefault => Scale ?? 1F;
    }
}