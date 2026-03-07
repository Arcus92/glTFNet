using System.Text.Json.Nodes;

namespace glTFNet.Generator;

/// <summary>
/// Defines a property of a <see cref="SchemaClass"/>.
/// </summary>
public class SchemaClassProperty
{
    /// <summary>
    /// Gets the property name.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the property type.
    /// </summary>
    public required ISchemaType Type { get; init; }
    
    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets if this property is required.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets the default value of the property.
    /// </summary>
    public JsonNode? Default { get; init; }
}