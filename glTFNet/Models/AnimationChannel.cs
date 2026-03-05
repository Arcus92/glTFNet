namespace glTFNet.Models
{
    /// <summary>
    /// An animation channel combines an animation sampler with a target property being animated.
    /// </summary>
    [System.Serializable]
    public class AnimationChannel : GlTFProperty
    {
        /// <summary>
        /// The index of a sampler in this animation used to compute the value for the target, e.g., a node's translation, rotation, or scale (TRS).
        /// </summary>
        public required System.Int32 Sampler { get; set; }

        /// <summary>
        /// The descriptor of the animated property.
        /// </summary>
        public required AnimationChannelTarget Target { get; set; }
    }
}