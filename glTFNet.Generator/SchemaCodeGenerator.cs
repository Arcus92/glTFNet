using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace glTFNet.Generator;

public class SchemaCodeGenerator
{
    /// <summary>
    /// Exports a schema file to the given directory.
    /// </summary>
    /// <param name="type">The type to generate.</param>
    /// <param name="outputPath">The output path.</param>
    public async Task Export(ISchemaGeneratedType type, string outputPath)
    {
        // Generate the code unit.
        var codeUnit = CreateCodeFromType(type);
        if (codeUnit is null)
            return;
        
        var fileName = $"{type.Name}.cs";
        
        // Formating code
        using var workspace = new AdhocWorkspace();
        var project = workspace.AddProject("Generator", LanguageNames.CSharp);
        var document = workspace.AddDocument(project.Id, fileName, SourceText.From(""));
        document = document.WithSyntaxRoot(codeUnit);
        document = await Formatter.FormatAsync(document);
        var formattedRoot = (await document.GetSyntaxRootAsync())!;
        
        // Writing to output
        var filePath = Path.Combine(outputPath, fileName);
        await using var textWriter = File.CreateText(filePath);
        formattedRoot.WriteTo(textWriter);
    }

    /// <summary>
    /// Generates a code unit from the given schema type.
    /// </summary>
    /// <param name="type">The type to generate.</param>
    /// <returns>Returns the whole compilation unit of the type.</returns>
    private CompilationUnitSyntax? CreateCodeFromType(ISchemaType type)
    {
        return type switch
        {
            SchemaClass schemaClass => CreateCodeFromClass(schemaClass),
            SchemaEnum schemaEnum => CreateCodeFromEnum(schemaEnum),
            _ => null
        };
    }

    /// <summary>
    /// Generates a code class from the given schema type.
    /// </summary>
    /// <param name="type">The class type to generate.</param>
    /// <returns>Returns the whole compilation unit of the class.</returns>
    private CompilationUnitSyntax CreateCodeFromClass(SchemaClass type)
    {
        var unit = SyntaxFactory.CompilationUnit();

        var schemaNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(type.Namespace));
        
        var schemaClass = SyntaxFactory.ClassDeclaration(type.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // Adding [Serializable]
        schemaClass = schemaClass.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.ParseName("System.Serializable")))));

        // Inheritance
        if (type.ParentClassType is not null)
        {
            schemaClass = schemaClass.AddBaseListTypes(SyntaxFactory.SimpleBaseType(type.ParentClassType.AsTypeSyntax()));
        }

        // Adding comments to class
        schemaClass = SchemaCodeGeneratorHelper.AddDocumentationSummery(schemaClass, type.Description);

        // Adding properties
        foreach (var property in type.Properties)
        {
            schemaClass = AddPropertyFromSchema(schemaClass, property);
        }

        schemaNamespace = schemaNamespace.AddMembers(schemaClass);
        unit = unit.AddMembers(schemaNamespace);
        return unit;
    }

    /// <summary>
    /// Generates a code class from the given schema type.
    /// </summary>
    /// <param name="type">The class type to generate.</param>
    /// <returns>Returns the whole compilation unit of the class.</returns>
    private CompilationUnitSyntax CreateCodeFromEnum(SchemaEnum type)
    {
        var unit = SyntaxFactory.CompilationUnit();

        var schemaNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(type.Namespace));
        
        var schemaEnum = SyntaxFactory.EnumDeclaration(type.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        foreach (var value in type.Values)
        {
            var schemaEnumMember = SyntaxFactory.EnumMemberDeclaration(value.Name);

            // Adding integer value
            if (value.IntegerValue.HasValue)
            {
                schemaEnumMember = schemaEnumMember.WithEqualsValue(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value.IntegerValue.Value))));
            }

            // Adding [JsonPropertyName("JSON_VALUE")]
            if (value.StringValue is not null)
            {
                schemaEnumMember = schemaEnumMember.AddAttributeLists(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Attribute(SyntaxFactory.ParseName("System.Text.Json.Serialization.JsonPropertyName"))
                                .WithArgumentList(SyntaxFactory.AttributeArgumentList()
                                    .AddArguments(SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value.StringValue))))))));
            }
            
            // Adding documentation
            schemaEnumMember = SchemaCodeGeneratorHelper.AddDocumentationSummery(schemaEnumMember, value.Description);
            
            schemaEnum = schemaEnum.AddMembers(schemaEnumMember);
        }
        
        schemaNamespace = schemaNamespace.AddMembers(schemaEnum);
        unit = unit.AddMembers(schemaNamespace);
        return unit;
    }


    /// <summary>
    /// Adds a schema property to the class definition.
    /// </summary>
    /// <param name="schemaClass">The class declaration to add the property to.</param>
    /// <param name="property">The property to add.</param>
    /// <returns>Returns the modified class declaration.</returns>
    private ClassDeclarationSyntax AddPropertyFromSchema(ClassDeclarationSyntax schemaClass, SchemaClassProperty property)
    {
        // Building property header
        var propertyTokens = new List<SyntaxToken>
        {
            SyntaxFactory.Token(SyntaxKind.PublicKeyword)
        };
        if (property.IsRequired)
        {
            propertyTokens.Add(SyntaxFactory.Token(SyntaxKind.RequiredKeyword));
        }

        var isNullable = !property.IsRequired;
        
        // Creating the property with default getter and setter.
        var propertyUnit = SyntaxFactory.PropertyDeclaration(isNullable ? property.Type.AsNullable().AsTypeSyntax() : property.Type.AsTypeSyntax(), property.Name)
            .AddModifiers(propertyTokens.ToArray())
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
        
        // Adding the default value for non-nullable properties
        var defaultExpression = SchemaCodeGeneratorHelper.GetExpressionSyntaxFromJsonNode(property.Default, property.Type);
        if (!isNullable && defaultExpression is not null)
        {
            propertyUnit = propertyUnit
                .WithInitializer(SyntaxFactory.EqualsValueClause(defaultExpression))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }
        
        // Adding comments to property
        propertyUnit = SchemaCodeGeneratorHelper.AddDocumentationSummery(propertyUnit, property.Description);

        schemaClass = schemaClass.AddMembers(propertyUnit);
        
        // Adding a second expression for nullable default values
        if (isNullable && defaultExpression is not null)
        {
            propertyUnit = SyntaxFactory.PropertyDeclaration(property.Type.AsTypeSyntax(),$"{property.Name}OrDefault")
                .AddAttributeLists(SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName("System.Text.Json.Serialization.JsonIgnore")))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.BinaryExpression(SyntaxKind.CoalesceExpression, SyntaxFactory.IdentifierName(property.Name), defaultExpression)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            propertyUnit = SchemaCodeGeneratorHelper.AddDocumentationInheritDoc(propertyUnit, property.Name);
            
            schemaClass = schemaClass.AddMembers(propertyUnit);
        }

        return schemaClass;
    }
}