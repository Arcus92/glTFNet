namespace glTFNet.Models.Extensions.Vendor.MpegMedia;

/// <summary>
/// Media used to create a texture, audio source, or any other media type.
/// </summary>
[Serializable]
public class MPEGMediaMedia : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// User-defined name of the media.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The startTime gives the time at which the rendering of the timed media will begin. The value is provided in seconds. In the case of timed textures, the static image should be rendered as a texture until the startTime is reached. A startTime of 0 means the presentation time of the current scene. Either startTime or autoplay shall be present in glTF description.
    /// </summary>
    public float? StartTime { get; set; }

    /// <inheritdoc cref="StartTime"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float StartTimeOrDefault => StartTime ?? 0F;

    /// <summary>
    /// The startTimeOffset indicates the time offset into the source, starting from which the timed media shall be generated. The value is provided in seconds, where 0 corresponds to the start of the source.
    /// </summary>
    public float? StartTimeOffset { get; set; }

    /// <inheritdoc cref="StartTimeOffset"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float StartTimeOffsetOrDefault => StartTimeOffset ?? 0F;

    /// <summary>
    /// The endTimeOffset indicates the end time offset into the source, up to which the timed media shall be generated. The value is provided in seconds. If not present, the endTimeOffset corresponds to the end of the source media.
    /// </summary>
    public float? EndTimeOffset { get; set; }

    /// <summary>
    /// Specifies that the media will start playing as soon as it is ready. Either startTime or autoplay shall be present for a media item description. Rendering of all media for which the autoplay flag is set to true should happen simultaneously.
    /// </summary>
    public bool? Autoplay { get; set; }

    /// <inheritdoc cref="Autoplay"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool AutoplayOrDefault => Autoplay ?? true;

    /// <summary>
    /// All media that have the same autoplayGroup identifier shall start playing synchronously as soon as all autoplayGroup media are ready. autoplayGroup is only allowed if autoplay is set to true.
    /// </summary>
    public int? AutoplayGroup { get; set; }

    /// <summary>
    /// Specifies that the media will start over again, every time it is finished. The timestamp in the buffer shall be continuously increasing when the media source loops, i.e. the playback duration prior to looping shall be added to the media time after looping.
    /// </summary>
    public bool? Loop { get; set; }

    /// <inheritdoc cref="Loop"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool LoopOrDefault => Loop ?? false;

    /// <summary>
    /// Specifies that media controls should be exposed to end user (such as a play/pause button etc).
    /// </summary>
    public bool? Controls { get; set; }

    /// <inheritdoc cref="Controls"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool ControlsOrDefault => Controls ?? false;

    /// <summary>
    /// An array of items that indicate alternatives of the same media (e.g. different video codecs used). The client could select items (i.e. uri and track) included in alternatives depending on the client’s capability.
    /// </summary>
    public required List<MPEGMediaMediaAlternative> Alternatives { get; set; }
}