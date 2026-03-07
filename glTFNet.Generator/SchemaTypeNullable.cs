namespace glTFNet.Generator;

/// <summary>
/// Describes a nullable wrapper for <see cref="ISchemaType"/>.
/// </summary>
/// <param name="baseType">The base type.</param>
public class SchemaTypeNullable(ISchemaType baseType) : ISchemaType
{
    /// <summary>
    /// Gets the base type of the nullable.
    /// </summary>
    public ISchemaType BaseType { get; } = baseType;
    
    /// <inheritdoc />
    public string FullName => $"{BaseType.FullName}?";
}