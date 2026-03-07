namespace glTFNet.Models
{
    public enum CameraType
    {
        [System.Text.Json.Serialization.JsonPropertyName("perspective")]
        Perspective = 0,
        [System.Text.Json.Serialization.JsonPropertyName("orthographic")]
        Orthographic = 0
    }
}