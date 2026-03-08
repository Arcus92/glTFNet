namespace glTFNet.Models.Extensions.KhrGaussianSplatting;

[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<MeshPrimitiveKHRGaussianSplattingColorSpace>))]
public enum MeshPrimitiveKHRGaussianSplattingColorSpace
{
    [System.Text.Json.Serialization.JsonStringEnumMemberName("srgb_rec709_display")]
    SrgbRec709Display,
    [System.Text.Json.Serialization.JsonStringEnumMemberName("lin_rec709_display")]
    LinRec709Display
}