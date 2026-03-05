using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using glTFNet.Generator.Models;
using glTFNet.Generator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace glTFNet.Generator;

public class SchemaCodeGenerator(JsonSchemaLoader loader, string outputPath, string outputNamespace)
{
    public async Task ExportAsync(string schemaId)
    {
        if (!loader.TryGetSchema(schemaId, out var schema))
        {
            throw new ArgumentException($"Schema could not be found in SchemaLoader: {schemaId}", nameof(schemaId));
        }

        await ExportAsync(schema);
    }

    public async Task ExportAsync(Schema schema)
    {
        
        var schemaClassName = GetCSharpClassName(schema.Id!);
        var schemaClassFile = CreateClassFromSchema(schema);
        var fileName = $"{schemaClassName}.cs";
        
        
        var filePath = Path.Combine(outputPath, fileName);
        
        using var workspace = new AdhocWorkspace();
        var project = workspace.AddProject("Generator", LanguageNames.CSharp);
        var document = workspace.AddDocument(project.Id, fileName, SourceText.From(""));
        document = document.WithSyntaxRoot(schemaClassFile);
        
        document = await Formatter.FormatAsync(document);
        
        var formattedRoot = (await document.GetSyntaxRootAsync())!;
        await using var textWriter = File.CreateText(filePath);
        formattedRoot.WriteTo(textWriter);
    }

    /// <summary>
    /// Generates a code class from the given schema definition.
    /// </summary>
    /// <param name="schema"></param>
    /// <returns>Returns the whole compilation unit of the class.</returns>
    private CompilationUnitSyntax CreateClassFromSchema(Schema schema)
    {
        var factory = SyntaxFactory.CompilationUnit();

        var schemaNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(outputNamespace));

        var className = GetCSharpClassName(schema.Id!);
        var schemaClass = SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // Adding [Serializable]
        schemaClass = schemaClass.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.ParseName("System.Serializable")))));

        // Inheritance
        if (schema.AllOf is not null && schema.AllOf.Length == 1)
        {
            var parentType = GetSchemaType(schema.AllOf[0], className);
            schemaClass = schemaClass.AddBaseListTypes(SyntaxFactory.SimpleBaseType(parentType.AsTypeSyntax()));
        }

        // Adding comments to class
        var classDescription = schema.DetailedDescription ?? schema.Description;
        schemaClass = CodeGeneratorHelper.AddDocumentationSummery(schemaClass, classDescription);

        // Adding properties
        if (schema.Properties is not null)
        {
            foreach (var (propertyName, propertySchema) in schema.Properties)
            {
                // Some properties are redefined in the child class but with an empty schema. These will be ignored.
                if (propertySchema.Type is null && propertySchema.Ref is null && propertySchema.AllOf is null &&
                    propertySchema.OneOf is null && propertySchema.AnyOf is null)
                {
                    continue;
                }

                var isRequired = IsPropertyRequiredInClass(schema, propertyName);
                schemaClass = AddPropertyFromSchema(schemaClass, propertyName, propertySchema, isRequired);
            }
        }

        schemaNamespace = schemaNamespace.AddMembers(schemaClass);
        factory = factory.AddMembers(schemaNamespace);
        return factory;
    }

    /// <summary>
    /// Returns if the given property is required in the given class schema.
    /// </summary>
    /// <param name="schema">The class schema to check for the property.</param>
    /// <param name="propertyName">The property name to check.</param>
    /// <returns>Returns true, if the property is required by this class.</returns>
    private static bool IsPropertyRequiredInClass(Schema schema, string propertyName)
    {
        if (!IsPropertyRequired(schema, propertyName))
        {
            return false;
        }
        
        // Check is this is excluded in some negated statement.
        if (schema.Not is not null)
        {
            if (IsPropertyRequiredInAny(schema.Not, propertyName))
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Returns if the given property is required in the given schema.
    /// </summary>
    /// <param name="schema">The class schema to check for the property.</param>
    /// <param name="propertyName">The property name to check.</param>
    /// <returns>Returns true, if the property is required by this schema.</returns>
    private static bool IsPropertyRequiredInAny(Schema schema, string propertyName)
    {
        if (IsPropertyRequired(schema, propertyName))
        {
            return true;
        }

        if (schema.AnyOf is not null)
        {
            foreach (var any in schema.AnyOf)
            {
                if (IsPropertyRequiredInAny(any, propertyName))
                {
                    return true;
                }
            }
        }
        
        if (schema.AllOf is not null)
        {
            foreach (var all in schema.AllOf)
            {
                if (IsPropertyRequiredInAny(all, propertyName))
                {
                    return true;
                }
            }
        }
        
        if (schema.OneOf is not null)
        {
            foreach (var one in schema.OneOf)
            {
                if (IsPropertyRequiredInAny(one, propertyName))
                {
                    return true;
                }
            }
        }

        return false;
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
    /// Adds a schema property to the class definition.
    /// </summary>
    /// <param name="schemaClass">The class declaration to add the property to.</param>
    /// <param name="name">The name in the schema of the property.</param>
    /// <param name="schema">The property type schema.</param>
    /// <param name="isRequired">Is set if this property is required by the object.</param>
    /// <returns>Returns the modified class declaration.</returns>
    private ClassDeclarationSyntax AddPropertyFromSchema(ClassDeclarationSyntax schemaClass, string name, Schema schema, bool isRequired)
    {
        // Building property header
        var propertyTokens = new List<SyntaxToken>()
        {
            SyntaxFactory.Token(SyntaxKind.PublicKeyword)
        };
        var propertyName = GetCSharpPropertyName(name);
        var propertyType = GetSchemaType(schema, propertyName);
        if (isRequired)
        {
            propertyTokens.Add(SyntaxFactory.Token(SyntaxKind.RequiredKeyword));
        }

        var isNullable = !isRequired;
        
        // Creating the property with default getter and setter.
        var property = SyntaxFactory.PropertyDeclaration(isNullable ? propertyType.AsNullable().AsTypeSyntax() : propertyType.AsTypeSyntax(), propertyName)
            .AddModifiers(propertyTokens.ToArray())
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
        
        // Adding the default value for non-nullable properties
        var defaultExpression = CodeGeneratorHelper.GetExpressionSyntaxFromJsonNode(schema.Default, propertyType);
        if (!isNullable && defaultExpression is not null)
        {
            property = property
                .WithInitializer(SyntaxFactory.EqualsValueClause(defaultExpression))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
        
        // Adding comments to property
        var propertyDescription = schema.DetailedDescription ?? schema.Description;
        property = CodeGeneratorHelper.AddDocumentationSummery(property, propertyDescription);

        schemaClass = schemaClass.AddMembers(property);
        
        // Adding a second expression for nullable default values
        if (isNullable && defaultExpression is not null)
        {
            property = SyntaxFactory.PropertyDeclaration(propertyType.AsTypeSyntax(),$"{propertyName}OrDefault")
                .AddAttributeLists(SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName("System.Text.Json.Serialization.JsonIgnore")))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.BinaryExpression(SyntaxKind.CoalesceExpression, SyntaxFactory.IdentifierName(propertyName), defaultExpression)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            property = CodeGeneratorHelper.AddDocumentationInheritDoc(property, propertyName);
            
            schemaClass = schemaClass.AddMembers(property);
        }

        return schemaClass;
    }
    
    
    /// <summary>
    /// Returns the C# type information of the given schema.
    /// </summary>
    /// <param name="schema">The schema to convert to a C# type.</param>
    /// <param name="typeHint">The property or class name for this type is used as additional information when choosing between similar types like Vector4 and Color.</param>
    /// <returns>Returns the C# type information.</returns>
    private SchemaTypeInfo GetSchemaType(Schema? schema, string? typeHint = null)
    {
        if (schema is null)
        {
            return SchemaTypeInfo.Object;
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
                        return SchemaTypeInfo.Quaternion;
                    }
                    return SchemaTypeInfo.Vector4;
                }
                if (schema is { MinItems: 3, MaxItems: 3 })
                {
                    return SchemaTypeInfo.Vector3;
                }
                if (schema is { MinItems: 2, MaxItems: 2 })
                {
                    return SchemaTypeInfo.Vector2;
                }
                if (schema is { MinItems: 16, MaxItems: 16 })
                {
                    return SchemaTypeInfo.Matrix4x4;
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
                return new SchemaTypeInfo(className);
            }
        }

        // Boolean
        if (schema.Type == "boolean")
        {
            return SchemaTypeInfo.Boolean;
        }
        
        // Integer
        if (schema.Type == "integer")
        {
            return SchemaTypeInfo.Integer;
        }

        // Number
        if (schema.Type == "number")
        {
            return SchemaTypeInfo.Single;
        }
        
        // String
        if (schema.Type == "string")
        {
            return SchemaTypeInfo.String;
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

        // Handle enum type
        if (TryGetEnumType(schema.AnyOf, out var enumType))
        {
            return enumType;
        }
        

        // Unknown
        return SchemaTypeInfo.Object;
    }

    /// <summary>
    /// Extracts the enum type name from a collection of 'AnyOf'.
    /// </summary>
    /// <param name="anyOf">The collection of AnyOf.</param>
    /// <param name="enumType">Returns the enum type info.</param>
    /// <returns>Returns true, if the type name could be extracted.</returns>
    private static bool TryGetEnumType(Schema[]? anyOf, out SchemaTypeInfo enumType)
    {
        enumType = default;
        if (anyOf is null || anyOf.Length == 0) return false;
        
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

            if (first)
            {
                first = false;
                sharedType = type;
                continue;
            }
            
            if (type == sharedType) continue;
            return false;
        }
        
        switch (sharedType)
        {
            case "boolean":
                enumType = SchemaTypeInfo.Boolean;
                return true;
            case "integer":
                enumType = SchemaTypeInfo.Integer;
                return true;
            case "number":
                enumType = SchemaTypeInfo.Single;
                return true;
            case "string":
                enumType = SchemaTypeInfo.String;
                return true;
        }

        return false;
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
}