namespace glTFNet.Generator.Types;

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
    public string GetName(SchemaTypeContext context)
    {
        // Handle short names
        if (Type == typeof(int)) return "int";
        if (Type == typeof(float)) return "float";
        if (Type == typeof(bool)) return "bool";
        if (Type == typeof(string)) return "string";
        if (Type == typeof(object)) return "object";
        
        if (context.Contains(Type.Namespace ?? ""))
            return Type.Name;

        return Type.FullName ?? Type.Name;
    }
    
    /// <inheritdoc />
    public override string ToString()
    {
        return Type.FullName ?? Type.Name;
    }
}