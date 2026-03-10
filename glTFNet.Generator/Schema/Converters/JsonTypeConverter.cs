using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFNet.Generator.Schema.Converters;

/// <summary>
/// A special converter for JSON schemas that support arrays.
/// </summary>
public class SchemaTypeConverter : JsonConverter<JsonSchemaTypeList>
{
    /// <inheritdoc />
    public override JsonSchemaTypeList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.StartArray:
                var types = JsonSerializer.Deserialize<string[]>(ref reader, options)!;
                return new JsonSchemaTypeList(types);
            case JsonTokenType.String:
                return new JsonSchemaTypeList(reader.GetString()!);
            default:
                throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, JsonSchemaTypeList value, JsonSerializerOptions options)
    {
        if (value.Types.Length == 1)
        {
            writer.WriteStringValue(value.Types[0]);
            return;
        }
        
        writer.WriteStartArray();
        foreach (var type in value.Types)
        {
            writer.WriteStringValue(type);
        }
        writer.WriteEndArray();
    }
}