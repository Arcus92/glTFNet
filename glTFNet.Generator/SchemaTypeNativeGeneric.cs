using System.Text;

namespace glTFNet.Generator;

/// <summary>
/// A reference to a generic type.
/// </summary>
/// <param name="type">The base type.</param>
/// <param name="genericTypes">The generic type arguments.</param>
public class SchemaTypeNativeGeneric(ISchemaType type, ISchemaType[] genericTypes) : ISchemaType
{
    /// <summary>
    /// Gets the internal type.
    /// </summary>
    public ISchemaType Type { get; } = type;
    
    /// <summary>
    /// Gets the generic types.
    /// </summary>
    public ISchemaType[] GenericTypes { get; } = genericTypes;
    
    /// <inheritdoc />
    public string GetName(SchemaTypeContext context)
    {
        var builder = new StringBuilder();

        var name = Type.GetName(context);
        builder.Append(name);
        
        // Removing the `{n} in the generic type name
        var index = name.IndexOf('`');
        if (index >= 0)
        {
            builder.Remove(index, builder.Length - index);
        }
        
        // Adding all generic types
        builder.Append('<');
        for (var i = 0; i < GenericTypes.Length; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }
            builder.Append(GenericTypes[i].GetName(context));
        }
        builder.Append('>');
        return builder.ToString();
        
    }
}