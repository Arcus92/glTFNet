namespace glTFNet.Generator;

/// <summary>
/// A reference to a native C# type.
/// </summary>
public class SchemaTypeNative(Type type) : ISchemaType
{
    /// <summary>
    /// Gets the internal type.
    /// </summary>
    public Type Type { get; } = type;
    
    /// <inheritdoc />
    public string FullName => Type.FullName ?? "";
}