namespace glTFNet.Models;

[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<AnimationChannelTargetPath>))]
public enum AnimationChannelTargetPath
{
    [System.Text.Json.Serialization.JsonStringEnumMemberName("translation")]
    Translation,
    [System.Text.Json.Serialization.JsonStringEnumMemberName("rotation")]
    Rotation,
    [System.Text.Json.Serialization.JsonStringEnumMemberName("scale")]
    Scale,
    [System.Text.Json.Serialization.JsonStringEnumMemberName("weights")]
    Weights
}