namespace glTFNet.Models.Extensions.KhrAnimationPointer;

/// <summary>
/// Extension object providing the JSON Pointer to the animated property.
/// </summary>
[Serializable]
public class AnimationChannelTargetKHRAnimationPointer : GlTFProperty
{
    /// <summary>
    /// JSON pointer to the animated property. The animation channel path value **MUST** be `pointer`.
    /// </summary>
    public required string Pointer { get; set; }
}