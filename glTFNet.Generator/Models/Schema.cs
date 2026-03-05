using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace glTFNet.Generator.Models;

/// <summary>
/// Defines a JSON schema.
/// </summary>
[Serializable]
public class Schema
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public Schema? Items { get; set; }
    public Schema[]? OneOf { get; set; }
    public Schema[]? AllOf { get; set; }
    public Schema[]? AnyOf { get; set; }
    public Schema? Not { get; set; }
    public JsonNode? Const { get; set; }
    public JsonNode? Default { get; set; }
    public int? MaxItems { get; set; }
    public int? MinItems { get; set; }
    public JsonNode? Minimum { get; set; }
    public JsonNode? Maximum { get; set; }
    public Dictionary<string, Schema>? Properties { get; set; }
    public string[]? Required { get; set; }
    
    // Custom properties
    
    [JsonPropertyName("gltf_detailedDescription")]
    public string? DetailedDescription { get; set; }
    [JsonPropertyName("gltf_webgl")]
    public string? WebGl { get; set; }
}