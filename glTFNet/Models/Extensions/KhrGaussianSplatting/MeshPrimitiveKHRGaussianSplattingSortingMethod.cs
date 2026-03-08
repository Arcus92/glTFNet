namespace glTFNet.Models.Extensions.KhrGaussianSplatting;

[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<MeshPrimitiveKHRGaussianSplattingSortingMethod>))]
public enum MeshPrimitiveKHRGaussianSplattingSortingMethod
{
    [System.Text.Json.Serialization.JsonStringEnumMemberName("cameraDistance")]
    Cameradistance
}