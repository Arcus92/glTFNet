using System.Text.Json;

namespace glTFNet.Generator.Schema;

public static class JsonSchemaLoader
{
    /// <summary>
    /// Loads a JSON schema from the given file path.
    /// </summary>
    /// <param name="path">The file path to the JSON schema.</param>
    /// <returns>Returns the loaded schema.</returns>
    public static async Task<JsonSchema> Load(string path)
    {
        await using var fileStream = File.OpenRead(path);
        var schema = await Load(fileStream);
        return schema;
    }

    /// <summary>
    /// Loads a JSON schema from the given stream.
    /// </summary>
    /// <param name="stream">The stream to the JSON schema.</param>
    /// <returns>Returns the loaded schema.</returns>
    public static async Task<JsonSchema> Load(Stream stream)
    {
        var schema = await JsonSerializer.DeserializeAsync(stream, JsonSchemaSerializerContext.Default.JsonSchema);
        if (schema is null)
        {
            throw new Exception("Failed to deserialize schema. Schema is null!");
        }
        
        return schema;
    }
}