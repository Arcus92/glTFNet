using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.Generator.Schema;
using glTFNet.Generator.Types;
using glTFNet.Generator.Utils;

namespace glTFNet.Generator.Analyser;

/// <summary>
/// An analyzer to extract class, properties and enums from multiple JSON schemas.
/// </summary>
public class SchemaAnalyser
{
    /// <summary>
    /// The JSON schema loader.
    /// </summary>
    private readonly JsonSchemaLoader _loader = new();
    
    /// <summary>
    /// The list of input files.
    /// </summary>
    private readonly List<SchemaAnalyserInput> _inputs = [];
    
    /// <summary>
    /// The found types by type name.
    /// </summary>
    private readonly List<ISchemaGeneratedType> _types = [];
    
    /// <summary>
    /// Gets all found types.
    /// </summary>
    public IReadOnlyList<ISchemaGeneratedType> Types => _types.AsReadOnly();

    /// <summary>
    /// The current input to analyze.
    /// </summary>
    private SchemaAnalyserInput _input = null!;
    
    /// <summary>
    /// Adds a schema to analyze.
    /// </summary>
    /// <param name="schema">The JSON schema.</param>
    /// <param name="ns">The destination namespace.</param>
    public void Add(JsonSchema schema, string ns)
    {
        _inputs.Add(new SchemaAnalyserInput
        {
            Schema = schema,
            Namespace = ns,
        });
    }
    
    /// <summary>
    /// Adds a schema to analyze.
    /// </summary>
    /// <param name="path">The JSON schema path.</param>
    /// <param name="ns">The destination namespace.</param>
    public async Task Add(string path, string ns)
    {
        var schema = await _loader.LoadSchema(path);
        Add(schema, ns);
    }
    
    /// <summary>
    /// Adds a all schemas from a directory to analyze.
    /// </summary>
    /// <param name="path">The JSON schema directory path.</param>
    /// <param name="ns">The destination namespace.</param>
    public async Task AddDirectory(string path, string ns)
    {
        await foreach (var schema in _loader.LoadSchemasFromDirectory(path))
        {
            Add(schema, ns);
        }
    }

    /// <summary>
    /// Analyses all schemas added with <see cref="Add"/>.
    /// </summary>
    public void Analyse()
    {
        foreach (var input in _inputs)
        {
            Analyse(input);
        }
    }
    
    /// <summary>
    /// Analyses the given schema.
    /// </summary>
    /// <param name="input">The schema to analyze.</param>
    private void Analyse(SchemaAnalyserInput input)
    {
        _input = input;
        var schema = input.Schema;

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
        // Inherit from dictionary if addition properties are allowed
        else if (schema.AdditionalProperties is not null)
        {
            var valueType = GetSchemaType(schema.AdditionalProperties);
            parentClass = SchemaType.Dictionary.MakeGenericType(SchemaType.String, valueType);
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
                var propertyType = GetSchemaType(propertySchema, className, propertyName);
                var propertyDescription = propertySchema.DetailedDescription ?? propertySchema.Description;
                
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
            Namespace = _input.Namespace,
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
    private static bool IsPropertyRequired(JsonSchema schema, string propertyName)
    {
        return schema.Required is not null && schema.Required.Contains(propertyName);
    }

    /// <summary>
    /// Returns the C# type information of the given schema.
    /// </summary>
    /// <param name="schema">The schema to convert to a C# type.</param>
    /// <param name="parentClassName">The parent class name of the type.</param>
    /// <param name="parentPropertyName">The parent property name of this type.</param>
    /// <returns>Returns the C# type information.</returns>
    private ISchemaType GetSchemaType(JsonSchema? schema, string? parentClassName = null, string? parentPropertyName = null)
    {
        if (schema is null)
        {
            return SchemaType.Object;
        }
        
        // Resolve type reference
        if (schema.Ref is not null)
        {
            if (schema.Ref.StartsWith("#/definitions/") && _input.Schema.Definitions is not null)
            {
                var definitionName = schema.Ref[14..];
                if (_input.Schema.Definitions.TryGetValue(definitionName, out var schemaDef))
                {
                    return GetSchemaType(schemaDef, parentClassName, parentPropertyName);
                }
            }
            if (_loader.TryGetSchema(schema.Ref, out var schemaRef))
            {
                return GetSchemaType(schemaRef, parentClassName, parentPropertyName);
            }

            throw new ArgumentException($"Schema could not be found in SchemaLoader: {schema.Ref}", nameof(schema));
            
        }

        // Array
        if (schema.Type == "array")
        {
            // Handle special numeric structs
            if (schema.Items?.Type == "number")
            {
                if (schema is { MinItems: 4, MaxItems: 4 })
                {
                    if (parentPropertyName == "Rotation")
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
            
            var itemType = GetSchemaType(schema.Items, parentClassName, parentPropertyName);
            return itemType.AsArray();
        }
        
        // Object
        if (schema.Type == "object")
        {
            if (schema.Id is not null)
            {
                var className = GetCSharpClassName(schema.Id);
                return new SchemaTypeReference(className, _input.Namespace);
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
                return GetSchemaType(schema.AllOf[0], parentClassName, parentPropertyName);
            }

            throw new NotSupportedException();
        }

        // Mark additional properties as string dictionary
        if (schema.AdditionalProperties is not null)
        {
            var valueType = GetSchemaType(schema.AdditionalProperties);
            return SchemaType.Dictionary.MakeGenericType(SchemaType.String, valueType);
        }
        
        // Handle enum type
        if (TryGetEnumType(schema.AnyOf, $"{parentClassName}{parentPropertyName}", out var enumType))
        {
            _types.Add(enumType);
            return enumType;
        }
        
        // Unknown
        return SchemaType.Object;
    }

    /// <summary>
    /// Extracts the enum type name from a collection of 'AnyOf'.
    /// </summary>
    /// <param name="anyOf">The collection of AnyOf.</param>
    /// <param name="enumName">The name of the enum.</param>
    /// <param name="enumType">Returns the enum type info.</param>
    /// <returns>Returns true, if the type name could be extracted.</returns>
    private bool TryGetEnumType(JsonSchema[]? anyOf, string enumName, [MaybeNullWhen(false)] out SchemaEnum enumType)
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
        
        enumType = new SchemaEnum
        {
            Name = enumName,
            Namespace = _input.Namespace,
            Type = enumValueType,
            Values = values
        };
        return true;
    }
    
    /// <summary>
    /// Returns the CSharp class name from the given schema id.
    /// </summary>
    /// <param name="schemaId">The <see cref="JsonSchema.Id"/>.</param>
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
    
    /// <summary>
    /// Returns the CSharp namespace name for the given schema name.
    /// </summary>
    /// <param name="schemaNamespace">The schema name.</param>
    /// <returns>Returns a CSharp namespace name.</returns>
    public static string GetCSharpNamespace(string schemaNamespace)
    {
        return schemaNamespace.ToLowerInvariant().ToPascalCase();
    }
}