using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFNet.Generator.Schema.Converters;

/// <summary>
/// A special converter for the AdditionalProperties parameter to handle a schema or a boolean.
/// </summary>
public class AdditionalPropertiesConverter : JsonConverter<JsonSchema>
{
    /// <inheritdoc />
    public override JsonSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.StartObject => JsonSerializer.Deserialize<JsonSchema>(ref reader, options),
            JsonTokenType.False => null,
            JsonTokenType.True => new JsonSchema(),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, JsonSchema value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}