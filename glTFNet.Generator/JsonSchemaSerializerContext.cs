using System.Text.Json.Serialization;
using glTFNet.Generator.Models;

namespace glTFNet.Generator;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Schema))]
public partial class JsonSchemaSerializerContext : JsonSerializerContext
{
}