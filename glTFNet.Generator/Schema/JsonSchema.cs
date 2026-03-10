using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using glTFNet.Generator.Schema.Converters;

namespace glTFNet.Generator.Schema;

/// <summary>
/// Defines a JSON schema.
/// </summary>
[Serializable]
public class JsonSchema
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public JsonSchemaTypeList? Type { get; set; }
    public JsonSchema? Items { get; set; }
    public JsonSchema[]? OneOf { get; set; }
    public JsonSchema[]? AllOf { get; set; }
    public JsonSchema[]? AnyOf { get; set; }
    public JsonSchema? Not { get; set; }
    public JsonNode? Const { get; set; }
    public JsonNode? Default { get; set; }
    public int? MaxItems { get; set; }
    public int? MinItems { get; set; }
    public int? MaxProperties { get; set; }
    public int? MinProperties { get; set; }
    public JsonNode? Minimum { get; set; }
    public JsonNode? Maximum { get; set; }
    public Dictionary<string, JsonSchema>? Properties { get; set; }
    public string[]? Required { get; set; }
    [JsonConverter(typeof(AdditionalPropertiesConverter))]
    public JsonSchema? AdditionalProperties { get; set; }
    public Dictionary<string, JsonSchema>? Definitions { get; set; }
    
    // Custom properties
    
    [JsonPropertyName("gltf_detailedDescription")]
    public string? DetailedDescription { get; set; }
    [JsonPropertyName("gltf_webgl")]
    public string? WebGl { get; set; }
}