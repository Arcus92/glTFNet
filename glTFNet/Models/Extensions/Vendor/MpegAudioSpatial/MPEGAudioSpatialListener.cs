namespace glTFNet.Models.Extensions.Vendor.MpegAudioSpatial;

[Serializable]
public class MPEGAudioSpatialListener : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// A unique identifier of the audio listener in the scene.
    /// </summary>
    public required int Id { get; set; }
}