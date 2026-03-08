namespace glTFNet.Models
{
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<CameraType>))]
    public enum CameraType
    {
        [System.Text.Json.Serialization.JsonStringEnumMemberName("perspective")]
        Perspective,
        [System.Text.Json.Serialization.JsonStringEnumMemberName("orthographic")]
        Orthographic
    }
}