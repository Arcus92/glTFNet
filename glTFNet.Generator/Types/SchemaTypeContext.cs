namespace glTFNet.Generator.Types;

/// <summary>
/// Defines the schema type's namespace and usings.
/// </summary>
public class SchemaTypeContext
{
    /// <summary>
    /// Gets the namespaces for the current file.
    /// </summary>
    public required string Namespace { get; set; }
    
    /// <summary>
    /// Gets the usings for the current file.
    /// </summary>
    public List<string> Usings { get; set; } = [];

    /// <summary>
    /// Checks if the given namespace is known in the current context.
    /// </summary>
    /// <param name="ns">The namespace to check.</param>
    /// <returns>Returns true, if the namespace is known.</returns>
    public bool Contains(string ns)
    {
        if (Namespace == ns)
        {
            return true;
        }
        
        return Usings.Contains(ns);
    }

    /// <summary>
    /// Gets an empty type context.
    /// </summary>
    public static SchemaTypeContext Empty { get; } = new() { Namespace = "" };
}