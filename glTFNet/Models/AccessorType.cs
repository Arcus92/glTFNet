namespace glTFNet.Models
{
    public enum AccessorType
    {
        [System.Text.Json.Serialization.JsonPropertyName("SCALAR")]
        Scalar,
        [System.Text.Json.Serialization.JsonPropertyName("VEC2")]
        Vec2,
        [System.Text.Json.Serialization.JsonPropertyName("VEC3")]
        Vec3,
        [System.Text.Json.Serialization.JsonPropertyName("VEC4")]
        Vec4,
        [System.Text.Json.Serialization.JsonPropertyName("MAT2")]
        Mat2,
        [System.Text.Json.Serialization.JsonPropertyName("MAT3")]
        Mat3,
        [System.Text.Json.Serialization.JsonPropertyName("MAT4")]
        Mat4
    }
}