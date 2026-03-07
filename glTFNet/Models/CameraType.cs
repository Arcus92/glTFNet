namespace glTFNet.Models
{
    public enum CameraType
    {
        [System.Text.Json.Serialization.JsonPropertyName("perspective")]
        Perspective,
        [System.Text.Json.Serialization.JsonPropertyName("orthographic")]
        Orthographic
    }
}