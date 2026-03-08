namespace glTFNet.Generator.Types;

/// <summary>
/// Defines an enum from the JSON schema.
/// </summary>
public class SchemaEnum : ISchemaGeneratedType
{
    /// <summary>
    /// Gets the enum name.
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
    /// Gets the enum type. Only string or integer are supported.
    /// </summary>
    public required ISchemaType Type { get; init; }
    
    /// <summary>
    /// Gets the enum values.
    /// </summary>
    public List<SchemaEnumValue> Values { get; init; } = [];
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Namespace}.{Name}";
    }
}