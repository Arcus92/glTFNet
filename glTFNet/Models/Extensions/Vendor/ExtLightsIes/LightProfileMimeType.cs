namespace glTFNet.Models.Extensions.Vendor.ExtLightsIes;

[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<LightProfileMimeType>))]
public enum LightProfileMimeType
{
    [System.Text.Json.Serialization.JsonStringEnumMemberName("application/x-ies-lm-63")]
    ApplicationXIesLm63
}