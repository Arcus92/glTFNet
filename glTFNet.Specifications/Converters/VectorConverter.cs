using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFNet.Specifications.Converters;

/// <summary>
/// A generic vector converter that writes the numeric values as JSON array.
/// </summary>
/// <typeparam name="T">The numeric vector type.</typeparam>
public abstract class VectorConverter<T> : JsonConverter<T> where T : struct
{
    /// <summary>
    /// Gets the number of numeric items.
    /// </summary>
    protected abstract int Size { get; }
    
    /// <summary>
    /// Gets the component in the given array.
    /// </summary>
    /// <param name="array">The numeric array.</param>
    /// <param name="index">The index to get.</param>
    /// <returns>Returns the component.</returns>
    protected abstract float GetComponent(ref T array, int index);

    /// <summary>
    /// Sets the component in the given array.
    /// </summary>
    /// <param name="array">The numeric array.</param>
    /// <param name="index">The index to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>Returns the component.</returns>
    protected abstract void SetComponent(ref T array, int index, float value);
    
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start array token.");
        }

        T value = default;
        var n = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                SetComponent(ref value, n++, reader.GetSingle());
            }
            else
            {
                throw new JsonException("Expected number or end array token.");
            }
        }
        
        if (n != Size)
        {
            throw new JsonException("Invalid array length.");
        }
        
        return value;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        for (var i = 0; i < Size; i++)
        {
            writer.WriteNumberValue(GetComponent(ref value, i));
        }
        writer.WriteEndArray();
    }
}