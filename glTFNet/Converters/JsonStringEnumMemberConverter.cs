using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFNet.Converters;

/// <summary>
/// Converts an enum to a JSON string and allows the string to be renamed via the <see cref="EnumMemberAttribute"/>.
/// </summary>
/// <remarks>
/// The JSON name to value mapping must be unique.
/// </remarks>
/// <typeparam name="T">The enum type.</typeparam>
public class JsonStringEnumMemberConverter<T> : JsonConverter<T> where T : struct, Enum
{
    /// <summary>
    /// Maps the JSON name to an enum value.
    /// </summary>
    private readonly Dictionary<string, T> _nameToValue;
    
    /// <summary>
    /// Maps the enum value to a JSON name.
    /// </summary>
    private readonly Dictionary<T, string> _valueToName;


    /// <summary>
    /// Creates a new instance of the enum converter with the default case-insensitive string comparer. 
    /// </summary>
    public JsonStringEnumMemberConverter() : this(StringComparer.InvariantCultureIgnoreCase)
    {
    }
    
    /// <summary>
    /// Create a new instance of the enum converter with the given string comparer.
    /// </summary>
    /// <param name="stringComparer">The string comparer to be used when converting a JSON string to the enum value.</param>
    public JsonStringEnumMemberConverter(StringComparer stringComparer)
    {
        _nameToValue = new Dictionary<string, T>(stringComparer);
        _valueToName = new Dictionary<T, string>();
        
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (T)field.GetValue(null)!;
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            var name = attribute?.Value ?? field.Name;
            _nameToValue.Add(name, value);
            _valueToName.Add(value, name);
        }
    }
    
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value.");
        }
        
        var stringValue = reader.GetString()!;
        if (_nameToValue.TryGetValue(stringValue, out var value))
        {
            return value;
        }

        throw new JsonException($"Could not parse '{stringValue}' to enum with type {typeof(T).Name}).");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (_valueToName.TryGetValue(value, out var stringValue))
        {
            writer.WriteStringValue(stringValue);
            return;
        }
        
        throw new JsonException($"Could not write '{value}' to enum string from type {typeof(T).Name}).");
    }
}