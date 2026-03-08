namespace glTFNet.Generator.Types;

/// <summary>
/// Describes a possible enum value of <see cref="ISchemaType"/>.
/// </summary>
public class SchemaEnumValue
{
    /// <summary>
    /// Gets name of the enum value.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the integer value.
    /// </summary>
    public int? IntegerValue { get; init; }
    
    /// <summary>
    /// Gets the string value.
    /// </summary>
    public string? StringValue { get; init; }
    
    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}