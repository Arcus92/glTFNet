namespace glTFNet.Generator;

/// <summary>
/// Defines a class from a JSON schema.
/// </summary>
public class SchemaClass : ISchemaGeneratedType
{
    /// <summary>
    /// Gets the class type name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the namespace.
    /// </summary>
    public required string Namespace { get; init; }
    
    /// <inheritdoc />
    public string GetName(SchemaTypeContext context)
    {
        if (context.Contains(Namespace))
            return Name;

        return $"{Namespace}.{Name}";
    }
    
    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the parent class.
    /// </summary>
    public ISchemaType? ParentClassType { get; init; }
    
    /// <summary>
    /// Gets the properties in this class.
    /// </summary>
    public List<SchemaClassProperty> Properties { get; init; } = [];
}