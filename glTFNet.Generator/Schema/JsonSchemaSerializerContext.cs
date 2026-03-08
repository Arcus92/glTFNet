using System.Text.Json.Serialization;

namespace glTFNet.Generator.Schema;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(JsonSchema))]
public partial class JsonSchemaSerializerContext : JsonSerializerContext
{
}