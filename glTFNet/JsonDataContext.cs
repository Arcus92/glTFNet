using System.Text.Json.Serialization;
using glTFNet.Models;

namespace glTFNet;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(GlTF))]
public partial class JsonDataContext : JsonSerializerContext
{
}