namespace glTFNet.Models
{
    public enum AnimationSamplerInterpolation
    {
        /// <summary>
        /// The animated values are linearly interpolated between keyframes. When targeting a rotation, spherical linear interpolation (slerp) **SHOULD** be used to interpolate quaternions. The number of output elements **MUST** equal the number of input elements.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("LINEAR")]
        Linear = 0,
        /// <summary>
        /// The animated values remain constant to the output of the first keyframe, until the next keyframe. The number of output elements **MUST** equal the number of input elements.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("STEP")]
        Step = 0,
        /// <summary>
        /// The animation's interpolation is computed using a cubic spline with specified tangents. The number of output elements **MUST** equal three times the number of input elements. For each input element, the output stores three elements, an in-tangent, a spline vertex, and an out-tangent. There **MUST** be at least two keyframes when using this interpolation.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("CUBICSPLINE")]
        Cubicspline = 0
    }
}