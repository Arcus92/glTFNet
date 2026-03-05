namespace glTFNet.Models
{
    /// <summary>
    /// A keyframe animation.
    /// </summary>
    [System.Serializable]
    public class Animation : GlTFChildOfRootProperty
    {
        /// <summary>
        /// An array of animation channels. An animation channel combines an animation sampler with a target property being animated. Different channels of the same animation **MUST NOT** have the same targets.
        /// </summary>
        public required AnimationChannel[] Channels { get; set; }

        /// <summary>
        /// An array of animation samplers. An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.
        /// </summary>
        public required AnimationSampler[] Samplers { get; set; }
    }
}