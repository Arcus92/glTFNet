namespace glTFNet.Models
{
    /// <summary>
    /// An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.
    /// </summary>
    [System.Serializable]
    public class AnimationSampler : GlTFProperty
    {
        /// <summary>
        /// The index of an accessor containing keyframe timestamps. The accessor **MUST** be of scalar type with floating-point components. The values represent time in seconds with `time[0] &gt;= 0.0`, and strictly increasing values, i.e., `time[n + 1] &gt; time[n]`.
        /// </summary>
        public required System.Int32 Input { get; set; }

        /// <summary>
        /// Interpolation algorithm.
        /// </summary>
        public System.String? Interpolation { get; set; }

        /// <inheritdoc cref="Interpolation"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.String InterpolationOrDefault => Interpolation ?? "LINEAR";

        /// <summary>
        /// The index of an accessor, containing keyframe output values.
        /// </summary>
        public required System.Int32 Output { get; set; }
    }
}