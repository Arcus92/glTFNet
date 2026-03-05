using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace glTFNet.Generator;

public static class CodeGeneratorHelper
{
    /// <summary>
    /// Gets the expression syntax from the given JSON value.
    /// </summary>
    /// <param name="jsonNode">The JSON value.</param>
    /// <param name="propertyType">The target property type name.</param>
    /// <returns>Returns an expression for this value if possible.</returns>
    public static ExpressionSyntax? GetExpressionSyntaxFromJsonNode(JsonNode? jsonNode, SchemaTypeInfo propertyType)
    {
        if (jsonNode is null)
        {
            return null;
        }
        
        return jsonNode.GetValueKind() switch
        {
            JsonValueKind.Null => SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression),
            JsonValueKind.False => SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression),
            JsonValueKind.True => SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
            JsonValueKind.String => SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(jsonNode.GetValue<string>())),
            JsonValueKind.Number when propertyType.Is<float>() => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(jsonNode.GetValue<float>())),
            JsonValueKind.Number => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(jsonNode.GetValue<int>())),
            JsonValueKind.Array when propertyType.Is<Vector2>() && jsonNode.AsArray().Count == 2 => SyntaxFactory.ImplicitObjectCreationExpression()
                .WithArgumentList(GetArgumentListFromArray(jsonNode.AsArray(), SchemaTypeInfo.Single)),
            JsonValueKind.Array when propertyType.Is<Vector3>() && jsonNode.AsArray().Count == 3 => SyntaxFactory.ImplicitObjectCreationExpression()
                .WithArgumentList(GetArgumentListFromArray(jsonNode.AsArray(), SchemaTypeInfo.Single)),
            JsonValueKind.Array when (propertyType.Is<Vector4>() || propertyType.Is<Quaternion>()) && jsonNode.AsArray().Count == 4 => SyntaxFactory.ImplicitObjectCreationExpression()
                .WithArgumentList(GetArgumentListFromArray(jsonNode.AsArray(), SchemaTypeInfo.Single)),
            JsonValueKind.Array when propertyType.Is<Matrix4x4>() && jsonNode.AsArray().Count == 16 => SyntaxFactory.ImplicitObjectCreationExpression()
                .WithArgumentList(GetArgumentListFromArray(jsonNode.AsArray(), SchemaTypeInfo.Single)),
            _ => null
        };
    }
    
    /// <summary>
    /// Creates an argument list from the given JSON value.
    /// </summary>
    /// <param name="jsonArray">The JSON values.</param>
    /// <param name="propertyType">The target property type for all values.</param>
    /// <returns>Returns the argument list.</returns>
    private static ArgumentListSyntax GetArgumentListFromArray(JsonArray jsonArray, SchemaTypeInfo propertyType)
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

    /// <summary>
    /// Adds a &lt;summery&gt; documentation comment on the given node. 
    /// </summary>
    /// <param name="syntaxNode">The node to add the documentation comment.</param>
    /// <param name="summery">The summery text.</param>
    /// <typeparam name="TSyntax">The syntax node type.</typeparam>
    /// <returns>Returns the modified syntax node.</returns>
    public static TSyntax AddDocumentationSummery<TSyntax>(TSyntax syntaxNode, string? summery) where TSyntax : SyntaxNode
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
    public static TSyntax AddDocumentationInheritDoc<TSyntax>(TSyntax syntaxNode, string? cref) where TSyntax : SyntaxNode
    {
        var attributes = SyntaxFactory.List<XmlAttributeSyntax>();
        
        if (!string.IsNullOrEmpty(cref))
        {
            attributes = attributes.Add(SyntaxFactory.XmlCrefAttribute(SyntaxFactory.NameMemberCref(SyntaxFactory.ParseTypeName(cref))));
        }
        
        var comment = SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
            .AddContent(
                SyntaxFactory.XmlEmptyElement(SyntaxFactory.XmlName("inheritdoc"), attributes)
            )
            .WithLeadingTrivia(SyntaxFactory.DocumentationCommentExterior("/// "));
        
        return syntaxNode.WithLeadingTrivia(SyntaxFactory.Trivia(comment), SyntaxFactory.ElasticCarriageReturnLineFeed);
    }
}