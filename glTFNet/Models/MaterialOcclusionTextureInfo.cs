namespace glTFNet.Models
{
    [System.Serializable]
    public class MaterialOcclusionTextureInfo : glTFNet.Models.TextureInfo
    {
        /// <summary>
        /// A scalar parameter controlling the amount of occlusion applied. A value of `0.0` means no occlusion. A value of `1.0` means full occlusion. This value affects the final occlusion value as: `1.0 + strength * (&lt;sampled occlusion texture value&gt; - 1.0)`.
        /// </summary>
        public System.Single? Strength { get; set; }

        /// <inheritdoc cref="Strength"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Single StrengthOrDefault => Strength ?? 1F;
    }
}