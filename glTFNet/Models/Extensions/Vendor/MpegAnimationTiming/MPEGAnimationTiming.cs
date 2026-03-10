namespace glTFNet.Models.Extensions.Vendor.MpegAnimationTiming;

/// <summary>
/// glTF extension to specify timing information that allow to synchronized animation with media
/// </summary>
[Serializable]
public class MPEGAnimationTiming : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// Provides a reference to `accessor`, by specifying the accessor's index in accessors array, that describes the buffer where the animation timing data will be made available. The sample format shall be as defined in ISO/IEC 23090-14:7.6.3. The componentType of the referenced accessor shall be `BYTE` and the type shall be `SCALAR`.
    /// </summary>
    public required int Accessor { get; set; }
}