namespace glTFNet.Models
{
    public enum AnimationChannelTargetPath
    {
        [System.Text.Json.Serialization.JsonPropertyName("translation")]
        Translation,
        [System.Text.Json.Serialization.JsonPropertyName("rotation")]
        Rotation,
        [System.Text.Json.Serialization.JsonPropertyName("scale")]
        Scale,
        [System.Text.Json.Serialization.JsonPropertyName("weights")]
        Weights
    }
}