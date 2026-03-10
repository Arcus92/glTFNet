using System.Text.Json.Serialization;
using glTFNet.Generator.Schema.Converters;

namespace glTFNet.Generator.Schema;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters =
    [
        typeof(SchemaTypeConverter)
    ]
)]
[JsonSerializable(typeof(JsonSchema))]
public partial class JsonSchemaSerializerContext : JsonSerializerContext
{
}