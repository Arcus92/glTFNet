namespace glTFNet.Generator.Schema;

/// <summary>
/// Allows to reference a <see cref="JsonSchema"/> with additional context, like class name of namespace.
/// </summary>
public struct JsonSchemaContext
{
    /// <summary>
    /// Gets the references schema.
    /// </summary>
    public required JsonSchema? Schema { get; set; }
    
    /// <summary>
    /// Gets the last root schema where the definitions are loaded from.
    /// </summary>
    public required JsonSchema? Root { get; set; }

    /// <summary>
    /// Gets the original referenced name.
    /// </summary>
    /// <remarks>
    /// The <see cref="ClassName"/> might change to avoid identical names since this isn't supported by the JSON
    /// serializer. Even in different namespaces.
    /// </remarks>
    public string? RefName { get; private set; }

    /// <summary>
    /// Gets the class name of this schema.
    /// </summary>
    public string? ClassName { get; private set; }
    
    /// <summary>
    /// Gets the namespace of this schema.
    /// </summary>
    public string? Namespace { get; private set; }
    
    /// <summary>
    /// Gets the parent property name.
    /// </summary>
    public string? ParentClassName { get; private set; }
    
    /// <summary>
    /// Gets the parent property name.
    /// </summary>
    public string? ParentPropertyName { get; private set; }

    /// <summary>
    /// Returns a new context with the reference name added.
    /// </summary>
    /// <param name="refName">The reference name to set.</param>
    /// <returns>Returns a new context.</returns>
    public JsonSchemaContext WithRefName(string refName)
    {
        var context = this;
        context.RefName = refName;
        return context;
    }
    
    /// <summary>
    /// Returns a new context with the class name added.
    /// </summary>
    /// <param name="className">The class name to set.</param>
    /// <returns>Returns a new context.</returns>
    public JsonSchemaContext WithClassName(string className)
    {
        var context = this;
        context.ClassName = className;
        context.ParentClassName = className;
        return context;
    }
    
    /// <summary>
    /// Returns a new context with the namespace added.
    /// </summary>
    /// <param name="ns">The namespace to set.</param>
    /// <returns>Returns a new context.</returns>
    public JsonSchemaContext WithNamespace(string ns)
    {
        var context = this;
        context.Namespace = ns;
        return context;
    }

    /// <summary>
    /// Create a new context for a new sub schema.
    /// </summary>
    /// <param name="schema">The sub schema.</param>
    /// <returns>Returns a new context.</returns>
    public JsonSchemaContext SubSchema(JsonSchema? schema)
    {
        var context = this;
        context.Schema = schema;
        context.ClassName = null;
        return context;
    }
    
    /// <summary>
    /// Create a new context for the given property.
    /// </summary>
    /// <param name="propertySchema">The property schema.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns>Returns a new context.</returns>
    public JsonSchemaContext Property(JsonSchema? propertySchema, string propertyName)
    {
        var context = this;
        context.Schema = propertySchema;
        context.ParentClassName = context.ClassName;
        context.ParentPropertyName = propertyName;
        context.ClassName = null;
        return context;
    }
    
    /// <summary>
    /// Creates a context for the given schema.
    /// </summary>
    /// <param name="schema">The schema.</param>
    /// <returns>Returns the schema context.</returns>
    public static JsonSchemaContext From(JsonSchema? schema)
    {
        return new JsonSchemaContext
        {
            Schema = schema,
            Root = schema
        };
    }
}