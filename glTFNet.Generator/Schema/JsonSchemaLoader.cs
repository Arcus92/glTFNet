using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace glTFNet.Generator.Schema;

public class JsonSchemaLoader : IReadOnlyCollection<JsonSchema>
{
    /// <summary>
    /// The registers schemas by id.
    /// </summary>
    private readonly Dictionary<string, JsonSchema> _schemas = new();

    /// <summary>
    /// Loads a JSON schema from the given file path.
    /// </summary>
    /// <param name="path">The file path to the JSON schema.</param>
    /// <returns>Returns the loaded schema.</returns>
    public async Task<JsonSchema> LoadSchema(string path)
    {
        await using var fileStream = File.OpenRead(path);
        var schema = await LoadSchema(fileStream, Path.GetFileName(path));
        return schema;
    }

    /// <summary>
    /// Loads a JSON schema from the given stream.
    /// </summary>
    /// <param name="stream">The stream to the JSON schema.</param>
    /// <param name="fileName">Optional filename to detect the schema id if missing from the JSON file.</param>
    /// <returns>Returns the loaded schema.</returns>
    public async Task<JsonSchema> LoadSchema(Stream stream, string? fileName)
    {
        var schema = await JsonSerializer.DeserializeAsync(stream, JsonSchemaSerializerContext.Default.JsonSchema);
        if (schema is null)
        {
            throw new Exception("Failed to deserialize schema. Schema is null!");
        }

        schema.Id ??= fileName;
        if (schema.Id is not null)
        {
            _schemas.Add(schema.Id, schema);
        }
        
        return schema;
    }

    /// <summary>
    /// Loads all JSON schema files in the given directory.
    /// </summary>
    /// <param name="path">The path to the directory.</param>
    /// <param name="searchOption">Optional directory search options.</param>
    /// <returns>Returns all loaded schemas.</returns>
    public async IAsyncEnumerable<JsonSchema> LoadSchemasFromDirectory(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        var files = Directory.EnumerateFiles(path, "*.schema.json", searchOption);
        foreach (var file in files)
        {
            yield return await LoadSchema(file);
        }
    }
    
    /// <summary>
    /// Tries to get a loaded schema by its id.
    /// </summary>
    /// <param name="id">The JSON schema id.</param>
    /// <param name="schema">Returns the JSON schema if found.</param>
    /// <returns>Returns true, if the id was found.</returns>
    public bool TryGetSchema(string id, [MaybeNullWhen(false)] out JsonSchema schema)
    {
        return _schemas.TryGetValue(id, out schema);
    }

    /// <summary>
    /// Clears the loader and removes all loaded schemas.
    /// </summary>
    public void Clear()
    {
        _schemas.Clear();
    }

    /// <inheritdoc />
    public IEnumerator<JsonSchema> GetEnumerator()
    {
        return _schemas.Values.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _schemas.Values.GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => _schemas.Count;
}