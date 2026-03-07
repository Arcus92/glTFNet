namespace glTFNet.Generator;

/// <summary>
/// Describes a reference to a generated type.
/// </summary>
/// <param name="name">The type name.</param>
/// <param name="ns">The namespace.</param>
public class SchemaTypeReference(string name, string ns) : ISchemaType
{
    /// <summary>
    /// Gets the type name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the namespace.
    /// </summary>
    public string Namespace { get; } = ns;
    
    /// <inheritdoc />
    public string FullName => $"{Namespace}.{Name}";
}