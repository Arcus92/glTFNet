using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.Generator.Models;
using glTFNet.Generator.Utils;

namespace glTFNet.Generator;

/// <summary>
/// An analyzer to extract class, properties and enums from multiple JSON schemas.
/// </summary>
public class SchemaAnalyser(JsonSchemaLoader loader, string ns)
{
    /// <summary>
    /// The found types by type name.
    /// </summary>
    private readonly List<ISchemaGeneratedType> _types = [];
    
    /// <summary>
    /// Gets all found types.
    /// </summary>
    public IReadOnlyList<ISchemaGeneratedType> Types => _types.AsReadOnly();

    /// <summary>
    /// Gets and sets the current namespace.
    /// </summary>
    public string Namespace { get; set; } = ns;
    
    /// <summary>
    /// Analyses the given schema.
    /// </summary>
    /// <param name="schema">The schema to analyze.</param>
    public void Analyse(Schema schema)
    {
        // All entry schema files should be a class with an id
        if (schema.Type != "object" || string.IsNullOrEmpty(schema.Id))
            return;

        var className = GetCSharpClassName(schema.Id);
        var classDescription = schema.DetailedDescription ?? schema.Description;
        var properties = new List<SchemaClassProperty>();
        
        // Inheritance
        ISchemaType? parentClass = null;
        if (schema.AllOf is not null && schema.AllOf.Length == 1)
        {
            parentClass = GetSchemaType(schema.AllOf[0], className);
        }
        
        // Adding properties
        if (schema.Properties is not null)
        {
            foreach (var (name, propertySchema) in schema.Properties)
            {
                // Some properties are redefined in the child class but with an empty schema. These will be ignored.
                if (propertySchema.Type is null && propertySchema.Ref is null && propertySchema.AllOf is null &&
                    propertySchema.OneOf is null && propertySchema.AnyOf is null)
                {
                    continue;
                }

                var isRequired = IsPropertyRequired(schema, name);
                
                var propertyName = GetCSharpPropertyName(name);
                var propertyType = GetSchemaType(propertySchema, propertyName);
                var propertyDescription = propertySchema.DetailedDescription ?? propertySchema.Description;
                
                // Handle enum type
                if (TryGetEnumType(propertySchema.AnyOf, className, propertyName, out var enumType))
                {
                    _types.Add(enumType);
                    propertyType = enumType;
                }
                
                properties.Add(new SchemaClassProperty
                {
                    Name = propertyName,
                    Type = propertyType,
                    Description = propertyDescription,
                    Default = propertySchema.Default,
                    IsRequired = isRequired
                });
            }
        }
        
        _types.Add(new SchemaClass
        {
            Name = className,
            Namespace = Namespace,
            Description = classDescription,
            ParentClassType = parentClass,
            Properties = properties
        });
    }
    
    /// <summary>
    /// Returns if the given property is required in the given schema.
    /// </summary>
    /// <param name="schema">The class schema to check for the property.</param>
    /// <param name="propertyName">The property name to check.</param>
    /// <returns>Returns true, if the property is required by this schema.</returns>
    private static bool IsPropertyRequired(Schema schema, string propertyName)
    {
        return schema.Required is not null && schema.Required.Contains(propertyName);
    }
    
    /// <summary>
    /// Returns the C# type information of the given schema.
    /// </summary>
    /// <param name="schema">The schema to convert to a C# type.</param>
    /// <param name="typeHint">The property or class name for this type is used as additional information when choosing between similar types like Vector4 and Color.</param>
    /// <returns>Returns the C# type information.</returns>
    private ISchemaType GetSchemaType(Schema? schema, string? typeHint = null)
    {
        if (schema is null)
        {
            return SchemaType.Object;
        }
        
        // Resolve type reference
        if (schema.Ref is not null)
        {
            if (!loader.TryGetSchema(schema.Ref, out var schemaRef))
            {
                throw new ArgumentException($"Schema could not be found in SchemaLoader: {schema.Ref}", nameof(schema));
            }

            return GetSchemaType(schemaRef);
        }

        // Array
        if (schema.Type == "array")
        {
            // Handle special numeric structs
            if (schema.Items?.Type == "number")
            {
                if (schema is { MinItems: 4, MaxItems: 4 })
                {
                    if (typeHint == "Rotation")
                    {
                        return SchemaType.Quaternion;
                    }
                    return SchemaType.Vector4;
                }
                if (schema is { MinItems: 3, MaxItems: 3 })
                {
                    return SchemaType.Vector3;
                }
                if (schema is { MinItems: 2, MaxItems: 2 })
                {
                    return SchemaType.Vector2;
                }
                if (schema is { MinItems: 16, MaxItems: 16 })
                {
                    return SchemaType.Matrix4x4;
                }
            }
            
            var itemType = GetSchemaType(schema.Items);
            return itemType.AsArray();
        }
        
        // Object
        if (schema.Type == "object")
        {
            if (schema.Id is not null)
            {
                var className = GetCSharpClassName(schema.Id);
                return new SchemaTypeReference(className, Namespace);
            }
        }

        // Boolean
        if (schema.Type == "boolean")
        {
            return SchemaType.Boolean;
        }
        
        // Integer
        if (schema.Type == "integer")
        {
            return SchemaType.Integer;
        }

        // Number
        if (schema.Type == "number")
        {
            return SchemaType.Single;
        }
        
        // String
        if (schema.Type == "string")
        {
            return SchemaType.String;
        }

        // Handle inherit type if nothing else matches
        if (schema.AllOf is not null)
        {
            if (schema.AllOf.Length == 1)
            {
                return GetSchemaType(schema.AllOf[0]);
            }

            throw new NotImplementedException();
        }

        // Mark additional properties as string dictionary
        if (schema.AdditionalProperties is not null)
        {
            var valueType = GetSchemaType(schema.AdditionalProperties);
            return SchemaType.Dictionary.MakeGenericType(SchemaType.String, valueType);
        }
        
        // Unknown
        return SchemaType.Object;
    }

    /// <summary>
    /// Extracts the enum type name from a collection of 'AnyOf'.
    /// </summary>
    /// <param name="anyOf">The collection of AnyOf.</param>
    /// <param name="className">The parent class name.</param>
    /// <param name="propertyName">The parent property name.</param>
    /// <param name="enumType">Returns the enum type info.</param>
    /// <returns>Returns true, if the type name could be extracted.</returns>
    private bool TryGetEnumType(Schema[]? anyOf, string className, string propertyName, [MaybeNullWhen(false)] out SchemaEnum enumType)
    {
        enumType = null;
        if (anyOf is null || anyOf.Length == 0) return false;

        var values = new List<SchemaEnumValue>();
        
        // Finding the common type for this value
        string? sharedType = null;
        var first = true;
        foreach (var any in anyOf)
        {
            var type = any.Type;
            
            // Get indirect type by const value
            if (type is null && any.Const is not null)
            {
                type = any.Const.GetValueKind() switch
                {
                    JsonValueKind.String => "string",
                    JsonValueKind.Number => "number",
                    _ => type
                };
            }

            // Collecting all possible values
            if (any.Const is JsonValue jsonValue)
            {
                jsonValue.TryGetValue<int?>(out var integerValue);
                jsonValue.TryGetValue<string>(out var stringValue);

                var valueName = stringValue ?? any.Description;
                if (!string.IsNullOrEmpty(valueName))
                {
                    var name = GetCSharpEnumName(valueName);
                    values.Add(new SchemaEnumValue
                    {
                        Name = name,
                        IntegerValue = integerValue,
                        StringValue = stringValue,
                        Description = any.Description
                    });
                }
            }
            
            if (first)
            {
                first = false;
                sharedType = type;
                continue;
            }
            
            if (type == sharedType) continue;
            return false;
        }

        ISchemaType enumValueType;
        switch (sharedType)
        {
            case "boolean":
                enumValueType = SchemaType.Boolean;
                break;
            case "integer":
                enumValueType = SchemaType.Integer;
                break;
            case "number":
                enumValueType = SchemaType.Single;
                break;
            case "string":
                enumValueType = SchemaType.String;
                break;
            default:
                return false;
        }

        var enumName = $"{className}{propertyName}";
        enumType = new SchemaEnum
        {
            Name = enumName,
            Namespace = Namespace,
            Type = enumValueType,
            Values = values
        };
        return true;
    }
    
    /// <summary>
    /// Returns the CSharp class name from the given schema id.
    /// </summary>
    /// <param name="schemaId">The <see cref="Schema.Id"/>.</param>
    /// <returns>Returns a CSharp class name.</returns>
    private static string GetCSharpClassName(string schemaId)
    {
        var name = schemaId;
        var index = schemaId.IndexOf(".schema.json", StringComparison.Ordinal);
        if (index >= 0)
        {
            name = name[..index];
        }
        
        return name.ToPascalCase();
    }

    /// <summary>
    /// Returns the CSharp property name for the given schema property name.
    /// </summary>
    /// <param name="schemaPropertyName">The schema property name.</param>
    /// <returns>Returns a CSharp property name.</returns>
    private static string GetCSharpPropertyName(string schemaPropertyName)
    {
        return schemaPropertyName.ToPascalCase();
    }

    /// <summary>
    /// Returns the CSharp enum name for the given schema enum name.
    /// </summary>
    /// <param name="schemaEnumName">The schema enum name.</param>
    /// <returns>Returns a CSharp enum name.</returns>
    private static string GetCSharpEnumName(string schemaEnumName)
    {
        return schemaEnumName.ToLowerInvariant().ToPascalCase();
    }
}