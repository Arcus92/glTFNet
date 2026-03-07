namespace glTFNet.Generator;

/// <summary>
/// Describes an array wrapper for <see cref="ISchemaType"/>.
/// </summary>
/// <param name="baseType">The base type.</param>
public class SchemaTypeArray(ISchemaType baseType) : ISchemaType
{
    /// <summary>
    /// Gets the base type of the array.
    /// </summary>
    public ISchemaType BaseType { get; } = baseType;
    
    /// <inheritdoc />
    public string FullName => $"{BaseType.FullName}[]";
}