using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using glTFNet.Generator.Models;

namespace glTFNet.Generator;

public class JsonSchemaLoader : IReadOnlyCollection<Schema>
{
    /// <summary>
    /// The registers schemas by id.
    /// </summary>
    private readonly Dictionary<string, Schema> _schemas = new();

    /// <summary>
    /// Loads a JSON schema from the given file path.
    /// </summary>
    /// <param name="path">The file path to the JSON schema.</param>
    /// <returns>Returns the loaded schema.</returns>
    public async Task<Schema> LoadSchema(string path)
    {
        await using var fileStream = File.OpenRead(path);
        return await LoadSchema(fileStream);
    }

    /// <summary>
    /// Loads a JSON schema from the given stream.
    /// </summary>
    /// <param name="stream">The stream to the JSON schema.</param>
    /// <returns>Returns the loaded schema.</returns>
    public async Task<Schema> LoadSchema(Stream stream)
    {
        var schema = await JsonSerializer.DeserializeAsync(stream, JsonSchemaSerializerContext.Default.Schema);
        if (schema is null)
        {
            throw new Exception("Failed to deserialize schema. Schema is null!");
        }

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
    /// <returns></returns>
    public async Task LoadSchemasFromDirectory(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        var files = Directory.EnumerateFiles(path, "*.schema.json", searchOption);
        foreach (var file in files)
        {
            await LoadSchema(file);
        }
    }
    
    /// <summary>
    /// Tries to get a loaded schema by its id.
    /// </summary>
    /// <param name="id">The JSON schema id.</param>
    /// <param name="schema">Returns the JSON schema if found.</param>
    /// <returns>Returns true, if the id was found.</returns>
    public bool TryGetSchema(string id, [MaybeNullWhen(false)] out Schema schema)
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
    public IEnumerator<Schema> GetEnumerator()
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