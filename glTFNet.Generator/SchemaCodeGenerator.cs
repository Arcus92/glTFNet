using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using glTFNet.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace glTFNet.Generator;

public class SchemaCodeGenerator
{
    private static readonly SchemaTypeNative TypeSerializableAttribute = new(typeof(SerializableAttribute));
    private static readonly SchemaTypeNative TypeJsonIgnoreAttribute = new(typeof(JsonIgnoreAttribute));
    private static readonly SchemaTypeNative TypeStringEnumMemberNameAttribute =
        new(typeof(JsonStringEnumMemberNameAttribute));
    private static readonly SchemaTypeNative TypeJsonConverterAttribute = new(typeof(JsonConverterAttribute));
    private static readonly SchemaTypeNative TypeJsonStringEnumConverter = new(typeof(JsonStringEnumConverter<>));

    /// <summary>
    /// The current type context.
    /// </summary>
    private SchemaTypeContext _context = SchemaTypeContext.Empty;

    /// <summary>
    /// Exports a schema file to the given directory.
    /// </summary>
    /// <param name="type">The type to generate.</param>
    /// <param name="rootPath">The output path.</param>
    public async Task Export(ISchemaGeneratedType type, string rootPath)
    {
        var outputPath = Path.Combine(rootPath, type.Namespace.Replace('.', '/'));
        Directory.CreateDirectory(outputPath);
        
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
    private CompilationUnitSyntax? CreateCodeFromType(ISchemaGeneratedType type)
    {
        _context = new SchemaTypeContext
        {
            Namespace = type.Namespace,
            Usings = ["System", "System.Collections.Generic"]
        };

        return type switch
        {
            SchemaClass schemaClass => CreateCodeFromClass(schemaClass),
            SchemaEnum schemaEnum => CreateCodeFromEnum(schemaEnum),
            _ => null
        };
    }

    #region Class
    
    /// <summary>
    /// Generates a code class from the given schema type.
    /// </summary>
    /// <param name="type">The class type to generate.</param>
    /// <returns>Returns the whole compilation unit of the class.</returns>
    private CompilationUnitSyntax CreateCodeFromClass(SchemaClass type)
    {
        var unit = SyntaxFactory.CompilationUnit();

        var schemaNamespace = SyntaxFactory.FileScopedNamespaceDeclaration(SyntaxFactory.ParseName(type.Namespace));

        var schemaClass = SyntaxFactory.ClassDeclaration(type.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // Adding [Serializable]
        schemaClass = schemaClass.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(TypeSerializableAttribute.AsAttributeSyntax(_context))));

        // Inheritance
        if (type.ParentClassType is not null)
        {
            schemaClass =
                schemaClass.AddBaseListTypes(SyntaxFactory.SimpleBaseType(type.ParentClassType.AsTypeSyntax(_context)));
        }

        // Adding comments to class
        schemaClass = AddDocumentationSummery(schemaClass, type.Description);

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
    /// Adds a schema property to the class definition.
    /// </summary>
    /// <param name="schemaClass">The class declaration to add the property to.</param>
    /// <param name="property">The property to add.</param>
    /// <returns>Returns the modified class declaration.</returns>
    private ClassDeclarationSyntax AddPropertyFromSchema(ClassDeclarationSyntax schemaClass,
        SchemaClassProperty property)
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
        var propertyUnit = SyntaxFactory
            .PropertyDeclaration(
                isNullable ? property.Type.AsNullable().AsTypeSyntax(_context) : property.Type.AsTypeSyntax(_context),
                property.Name)
            .AddModifiers(propertyTokens.ToArray())
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

        // Adding the default value for non-nullable properties
        var defaultExpression = GetExpressionSyntaxFromJsonNode(property.Default, property.Type);
        if (!isNullable && defaultExpression is not null)
        {
            propertyUnit = propertyUnit
                .WithInitializer(SyntaxFactory.EqualsValueClause(defaultExpression))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        // Adding comments to property
        propertyUnit = AddDocumentationSummery(propertyUnit, property.Description);

        schemaClass = schemaClass.AddMembers(propertyUnit);

        // Adding a second expression for nullable default values
        if (isNullable && defaultExpression is not null)
        {
            propertyUnit = SyntaxFactory
                .PropertyDeclaration(property.Type.AsTypeSyntax(_context), $"{property.Name}OrDefault")
                .AddAttributeLists(SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(TypeJsonIgnoreAttribute.AsAttributeSyntax(_context))))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.BinaryExpression(
                    SyntaxKind.CoalesceExpression, SyntaxFactory.IdentifierName(property.Name), defaultExpression)))
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            propertyUnit = AddDocumentationInheritDoc(propertyUnit, property.Name);

            schemaClass = schemaClass.AddMembers(propertyUnit);
        }

        return schemaClass;
    }
    
    #endregion Class

    #region Enum

    /// <summary>
    /// Generates a code class from the given schema type.
    /// </summary>
    /// <param name="type">The class type to generate.</param>
    /// <returns>Returns the whole compilation unit of the class.</returns>
    private CompilationUnitSyntax CreateCodeFromEnum(SchemaEnum type)
    {
        var unit = SyntaxFactory.CompilationUnit();

        var schemaNamespace = SyntaxFactory.FileScopedNamespaceDeclaration(SyntaxFactory.ParseName(type.Namespace));

        var schemaEnum = SyntaxFactory.EnumDeclaration(type.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // Adding the string enum converter
        if (type.Type.Is<string>())
        {
            schemaEnum = schemaEnum.AddAttributeLists(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(TypeJsonConverterAttribute.AsAttributeSyntax(_context)
                        .WithArgumentList(SyntaxFactory.AttributeArgumentList()
                            .AddArguments(SyntaxFactory.AttributeArgument(
                                    SyntaxFactory.TypeOfExpression(
                                        TypeJsonStringEnumConverter.MakeGenericType(type).AsTypeSyntax(_context))
                                )
                            )))));
        }

        // Adding all values
        foreach (var value in type.Values)
        {
            var schemaEnumMember = SyntaxFactory.EnumMemberDeclaration(value.Name);

            // Adding integer value
            if (value.IntegerValue.HasValue)
            {
                schemaEnumMember = schemaEnumMember.WithEqualsValue(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(value.IntegerValue.Value))));
            }

            // Adding [StringEnumMemberName("JSON_VALUE")]
            if (value.StringValue is not null)
            {
                schemaEnumMember = schemaEnumMember.AddAttributeLists(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList(TypeStringEnumMemberNameAttribute
                            .AsAttributeSyntax(_context)
                            .WithArgumentList(SyntaxFactory.AttributeArgumentList()
                                .AddArguments(SyntaxFactory.AttributeArgument(
                                    SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(value.StringValue))))))));
            }

            // Adding documentation
            schemaEnumMember = AddDocumentationSummery(schemaEnumMember, value.Description);

            schemaEnum = schemaEnum.AddMembers(schemaEnumMember);
        }

        schemaNamespace = schemaNamespace.AddMembers(schemaEnum);
        unit = unit.AddMembers(schemaNamespace);
        return unit;
    }

    #endregion Enum

    #region Value expression
    
    /// <summary>
    /// Gets the expression syntax from the given JSON value.
    /// </summary>
    /// <param name="jsonNode">The JSON value.</param>
    /// <param name="propertyType">The target property type name.</param>
    /// <returns>Returns an expression for this value if possible.</returns>
    private ExpressionSyntax? GetExpressionSyntaxFromJsonNode(JsonNode? jsonNode, ISchemaType propertyType)
    {
        if (jsonNode is null)
        {
            return null;
        }

        // Handle enum type
        if (propertyType is SchemaEnum schemaEnum && jsonNode is JsonValue jsonValue)
        {
            jsonValue.TryGetValue<int?>(out var integerValue);
            jsonValue.TryGetValue<string>(out var stringValue);

            // Find matching enum value
            foreach (var value in schemaEnum.Values)
            {
                if (value.IntegerValue == integerValue && value.StringValue == stringValue)
                {
                    return SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(schemaEnum.GetName(_context)),
                        SyntaxFactory.IdentifierName(value.Name)
                    );
                }
            }
        }

        return jsonNode.GetValueKind() switch
        {
            JsonValueKind.Null => SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression),
            JsonValueKind.False => SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression),
            JsonValueKind.True => SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
            JsonValueKind.String => SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(jsonNode.GetValue<string>())),
            JsonValueKind.Number when propertyType.Is<float>() => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(jsonNode.GetValue<float>())),
            JsonValueKind.Number => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(jsonNode.GetValue<int>())),
            JsonValueKind.Array when propertyType.Is<Vector2>() && jsonNode.AsArray().Count == 2 => SyntaxFactory
                .ImplicitObjectCreationExpression()
                .WithArgumentList(GetVectorArgumentListFromArray(jsonNode.AsArray(), SchemaType.Single)),
            JsonValueKind.Array when propertyType.Is<Vector3>() && jsonNode.AsArray().Count == 3 => SyntaxFactory
                .ImplicitObjectCreationExpression()
                .WithArgumentList(GetVectorArgumentListFromArray(jsonNode.AsArray(), SchemaType.Single)),
            JsonValueKind.Array when (propertyType.Is<Vector4>() || propertyType.Is<Quaternion>()) &&
                                     jsonNode.AsArray().Count == 4 => SyntaxFactory.ImplicitObjectCreationExpression()
                .WithArgumentList(GetVectorArgumentListFromArray(jsonNode.AsArray(), SchemaType.Single)),
            JsonValueKind.Array when propertyType.Is<Matrix4x4>() && jsonNode.AsArray().Count == 16 => SyntaxFactory
                .ImplicitObjectCreationExpression()
                .WithArgumentList(GetVectorArgumentListFromArray(jsonNode.AsArray(), SchemaType.Single)),
            _ => null
        };
    }

    /// <summary>
    /// Creates an argument list from the given JSON value.
    /// </summary>
    /// <param name="jsonArray">The JSON values.</param>
    /// <param name="propertyType">The target property type for all values.</param>
    /// <returns>Returns the argument list.</returns>
    private ArgumentListSyntax GetVectorArgumentListFromArray(JsonArray jsonArray, ISchemaType propertyType)
    {
        var argumentList = SyntaxFactory.ArgumentList();

        foreach (var jsonNode in jsonArray)
        {
            argumentList =
                argumentList.AddArguments(
                    SyntaxFactory.Argument(GetExpressionSyntaxFromJsonNode(jsonNode!, propertyType)!));
        }

        return argumentList;
    }
    
    #endregion Value expression

    #region Documentation

    /// <summary>
    /// Adds a &lt;summery&gt; documentation comment on the given node. 
    /// </summary>
    /// <param name="syntaxNode">The node to add the documentation comment.</param>
    /// <param name="summery">The summery text.</param>
    /// <typeparam name="TSyntax">The syntax node type.</typeparam>
    /// <returns>Returns the modified syntax node.</returns>
    private static TSyntax AddDocumentationSummery<TSyntax>(TSyntax syntaxNode, string? summery)
        where TSyntax : SyntaxNode
    {
        if (string.IsNullOrEmpty(summery))
        {
            return syntaxNode;
        }

        var tokens = new List<SyntaxToken>();
        foreach (var line in summery.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            tokens.Add(SyntaxFactory.XmlTextNewLine(Environment.NewLine));
            tokens.Add(SyntaxFactory.XmlTextLiteral(line.Trim()));
        }

        tokens.Add(SyntaxFactory.XmlTextNewLine(Environment.NewLine));

        var comment = SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
            .AddContent(
                SyntaxFactory.XmlSummaryElement(SyntaxFactory.List<XmlNodeSyntax>()
                    .Add(SyntaxFactory.XmlText().AddTextTokens(tokens.ToArray()))
                )
            )
            .WithLeadingTrivia(SyntaxFactory.DocumentationCommentExterior("/// "));

        return syntaxNode.WithLeadingTrivia(SyntaxFactory.Trivia(comment), SyntaxFactory.ElasticCarriageReturnLineFeed);
    }

    /// <summary>
    /// Adds a &lt;inheritdoc&gt; documentation comment on the given node. 
    /// </summary>
    /// <param name="syntaxNode">The node to add the documentation comment.</param>
    /// <param name="cref">The optional cref.</param>
    /// <typeparam name="TSyntax">The syntax node type.</typeparam>
    /// <returns>Returns the modified syntax node.</returns>
    private static TSyntax AddDocumentationInheritDoc<TSyntax>(TSyntax syntaxNode, string? cref)
        where TSyntax : SyntaxNode
    {
        var attributes = SyntaxFactory.List<XmlAttributeSyntax>();

        if (!string.IsNullOrEmpty(cref))
        {
            attributes =
                attributes.Add(
                    SyntaxFactory.XmlCrefAttribute(SyntaxFactory.NameMemberCref(SyntaxFactory.ParseTypeName(cref))));
        }

        var comment = SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
            .AddContent(
                SyntaxFactory.XmlEmptyElement(SyntaxFactory.XmlName("inheritdoc"), attributes)
            )
            .WithLeadingTrivia(SyntaxFactory.DocumentationCommentExterior("/// "));

        return syntaxNode.WithLeadingTrivia(SyntaxFactory.Trivia(comment), SyntaxFactory.ElasticCarriageReturnLineFeed);
    }
    
    #endregion Documentation
}