using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.Generator.Schema;
using glTFNet.Generator.Types;
using glTFNet.Generator.Utils;
using JetBrains.Annotations;

namespace glTFNet.Generator.Analyser;

/// <summary>
/// An analyzer to extract class, properties and enums from multiple JSON schemas.
/// </summary>
[PublicAPI]
public class SchemaAnalyser
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
    /// The list of input files.
    /// </summary>
    private readonly List<JsonSchemaContext> _schemas = [];
    
    /// <summary>
    /// Adds a schema to analyze.
    /// </summary>
    /// <param name="schema">The JSON schema.</param>
    /// <param name="className">The schema class name.</param>
    /// <param name="ns">The destination namespace.</param>
    public void Add(JsonSchema schema, string className, string ns)
    {
        _schemas.Add(JsonSchemaContext.From(schema).WithClassName(className).WithNamespace(ns));
    }
    
    /// <summary>
    /// Adds a schema to analyze.
    /// </summary>
    /// <param name="path">The JSON schema path.</param>
    /// <param name="ns">The destination namespace.</param>
    public async Task Add(string path, string ns)
    {
        var schema = await JsonSchemaLoader.Load(path);
        var fileName = Path.GetFileName(path);
        var className = GetCSharpClassName(fileName);
        Add(schema, className, ns);
    }

    /// <summary>
    /// Adds an all schemas from a directory to analyze.
    /// </summary>
    /// <param name="path">The JSON schema directory path.</param>
    /// <param name="ns">The destination namespace.</param>
    /// <param name="searchOption">The optional directory search options.</param>
    public async Task AddDirectory(string path, string ns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*.schema.json", searchOption))
        {
            await Add(file, ns);
        }
    }

    /// <summary>
    /// Analyses all schemas added with Add or AddDirectory.
    /// </summary>
    public void Analyse()
    {
        foreach (var input in _schemas)
        {
            Analyse(input);
        }
    }
    
    /// <summary>
    /// Analyses the given schema.
    /// </summary>
    /// <param name="context">The schema to analyze.</param>
    private void Analyse(JsonSchemaContext context)
    {
        var schema = context.Schema;
        if (schema is null)
            return;

        // All entry schema files should be a class with an id
        if (schema.Type != "object" || context.ClassName is null)
            return;

        var className = context.ClassName;
        var classDescription = schema.DetailedDescription ?? schema.Description;
        var properties = new List<SchemaClassProperty>();
        
        // Inheritance
        ISchemaType? parentClass = null;
        if (schema.AllOf is not null && schema.AllOf.Length == 1)
        {
            parentClass = GetSchemaType(context.SubSchema(schema.AllOf[0]));
        }
        // Inherit from dictionary if addition properties are allowed
        else if (schema.AdditionalProperties is not null)
        {
            var valueType = GetSchemaType(context.SubSchema(schema.AdditionalProperties));
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
                var propertyType = GetSchemaType(context.Property(propertySchema, propertyName));
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
            Namespace = context.Namespace ?? "",
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
    /// <param name="context">The schema context to convert to a C# type.</param>
    /// <returns>Returns the C# type information.</returns>
    private ISchemaType GetSchemaType(JsonSchemaContext context)
    {
        var schema = context.Schema;
        if (schema is null)
        {
            return SchemaType.Object;
        }
        
        // Resolve type reference
        if (schema.Ref is not null)
        {
            // References a definition inside the current schema.
            if (schema.Ref.StartsWith("#/definitions/") && context.Root?.Definitions is not null)
            {
                var definitionName = schema.Ref[14..];
                if (context.Root.Definitions.TryGetValue(definitionName, out var schemaDef))
                {
                    return GetSchemaType(context.SubSchema(schemaDef));
                }
            }

            // References an external schema
            var refClassName = GetCSharpClassName(schema.Ref);
            if (TryGetExternalSchema(refClassName, context.Namespace, out var refSchema))
            {
                return GetSchemaType(refSchema);
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
                    if (context.ParentPropertyName == "Rotation")
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
            
            var itemType = GetSchemaType(context.SubSchema(schema.Items));
            return itemType.AsArray();
        }
        
        // Object
        if (schema.Type == "object")
        {
            if (context.ClassName is not null && context.Namespace is not null)
            {
                return new SchemaTypeReference(context.ClassName, context.Namespace);
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
                return GetSchemaType(context.SubSchema(schema.AllOf[0]));
            }

            throw new NotSupportedException();
        }

        // Mark additional properties as string dictionary
        if (schema.AdditionalProperties is not null)
        {
            var valueType = GetSchemaType(context.SubSchema(schema.AdditionalProperties));
            return SchemaType.Dictionary.MakeGenericType(SchemaType.String, valueType);
        }
        
        // Handle enum type
        if (TryGetEnumType(context, out var enumType))
        {
            _types.Add(enumType);
            return enumType;
        }
        
        // Unknown
        return SchemaType.Object;
    }
    
    /// <summary>
    /// Tries to get a known schema context by name and namespace.
    /// It looks for a matching schema in the given namespace first. If nothing was found, it searches all in namespaces.
    /// </summary>
    /// <param name="name">The class name.</param>
    /// <param name="ns">The current namespace to look into.</param>
    /// <param name="result">Returns the found schema context.</param>
    /// <returns>Returns true, if the schema was found.</returns>
    private bool TryGetExternalSchema(string name, string? ns, out JsonSchemaContext result)
    {
        result = _schemas.FirstOrDefault(x => x.ClassName == name && x.Namespace == ns);
        if (result.Schema is not null) return true;
        
        result = _schemas.FirstOrDefault(x => x.ClassName == name);
        if (result.Schema is not null) return true;
        
        result = default;
        return false;
    }

    /// <summary>
    /// Extracts the enum type name from a collection of 'AnyOf'.
    /// </summary>
    /// <param name="context">The current schema context.</param>
    /// <param name="enumType">Returns the enum type info.</param>
    /// <returns>Returns true, if the type name could be extracted.</returns>
    private bool TryGetEnumType(JsonSchemaContext context, [MaybeNullWhen(false)] out SchemaEnum enumType)
    {
        var anyOf = context.Schema?.AnyOf;
        
        enumType = null;
        if (anyOf is null || anyOf.Length == 0) return false;

        var values = new List<SchemaEnumValue>();
        
        // Finding the common type for this value
        string? sharedType = null;
        var first = true;
        foreach (var any in anyOf)
        {
            var type = any.Type?.Types.FirstOrDefault();
            
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
            Name = $"{context.ParentClassName}{context.ParentPropertyName}",
            Namespace = context.Namespace ?? "",
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