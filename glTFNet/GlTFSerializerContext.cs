using System.Text.Json.Serialization;
using glTFNet.Converters;
using glTFNet.Models;

namespace glTFNet;

/// <summary>
/// The JSON serializer context 
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters = [
        typeof(Vector2Converter), 
        typeof(Vector3Converter), 
        typeof(Vector4Converter),
        typeof(QuaternionConverter),
        typeof(Matrix4x4Converter)
    ]
)]
[JsonSerializable(typeof(GlTF))]
// ReSharper disable once InconsistentNaming
public partial class GlTFSerializerContext : JsonSerializerContext;