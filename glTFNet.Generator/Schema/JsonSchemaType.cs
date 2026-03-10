namespace glTFNet.Generator.Schema;

/// <summary>
/// Defines a list for multiple entries in the <see cref="JsonSchema.Type"/> property.
/// </summary>
/// <param name="Types"></param>
public record struct JsonSchemaTypeList(params string[] Types)
{
    /// <summary>
    /// Returns if this type list has one value and matches the given type.
    /// </summary>
    /// <param name="type">The type name to match.</param>
    /// <returns>Returns true, if the type matches the type list.</returns>
    public bool Is(string type)
    {
        return Types.Length == 1 &&  Types[0] == type;
    }

    public static bool operator ==(JsonSchemaTypeList? list, string? type)
    {
        if (list is null && type is null) return true;
        if (list is null || type is null) return false;
        
        return list.Value.Is(type);
    }
    
    public static bool operator !=(JsonSchemaTypeList? list, string? type)
    {
        if (list is null && type is null) return false;
        if (list is null || type is null) return true;
        
        return !list.Value.Is(type);
    }
}