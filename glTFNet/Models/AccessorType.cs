namespace glTFNet.Models
{
    public enum AccessorType
    {
        [System.Text.Json.Serialization.JsonPropertyName("SCALAR")]
        Scalar = 0,
        [System.Text.Json.Serialization.JsonPropertyName("VEC2")]
        Vec2 = 0,
        [System.Text.Json.Serialization.JsonPropertyName("VEC3")]
        Vec3 = 0,
        [System.Text.Json.Serialization.JsonPropertyName("VEC4")]
        Vec4 = 0,
        [System.Text.Json.Serialization.JsonPropertyName("MAT2")]
        Mat2 = 0,
        [System.Text.Json.Serialization.JsonPropertyName("MAT3")]
        Mat3 = 0,
        [System.Text.Json.Serialization.JsonPropertyName("MAT4")]
        Mat4 = 0
    }
}