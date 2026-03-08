using System.Numerics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace glTFNet.Generator.Types;

/// <summary>
/// Describes a referenced type by a schema.
/// </summary>
public interface ISchemaType
{
    /// <summary>
    /// Gets the C# type name in the given context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Returns the type name.</returns>
    string GetName(SchemaTypeContext context);
}

/// <summary>
/// Helper class for <see cref="ISchemaType"/>.
/// </summary>
public static class SchemaType
{
    /// <param name="schemaType">The schema type.</param>
    extension(ISchemaType schemaType)
    {
        /// <summary>
        /// Creates an array type information of this type.
        /// </summary>
        /// <returns>Returns the new array type.</returns>
        public ISchemaType AsArray()
        {
            return new SchemaTypeArray(schemaType);
        }

        /// <summary>
        /// Creates a nullable type information of this type.
        /// </summary>
        /// <returns>Returns the new nullable type.</returns>
        public ISchemaType AsNullable()
        {
            return new SchemaTypeNullable(schemaType);
        }

        /// <summary>
        /// Returns the type syntax.
        /// </summary>
        /// <param name="context">The current type context.</param>
        /// <returns>Returns the type syntax.</returns>
        public TypeSyntax AsTypeSyntax(SchemaTypeContext context)
        {
            return SyntaxFactory.ParseTypeName(schemaType.GetName(context));
        }

        /// <summary>
        /// Returns the attribute syntax name.
        /// </summary>
        /// <param name="context">The current type context.</param>
        /// <returns>Returns the attribute syntax.</returns>
        public AttributeSyntax AsAttributeSyntax(SchemaTypeContext context)
        {
            var name = schemaType.GetName(context);
            if (name.EndsWith("Attribute"))
            {
                name = name[..^9];
            }
            return SyntaxFactory.Attribute(SyntaxFactory.ParseName(name));
        }
        
        /// <summary>
        /// Returns a generic type reference from this type.
        /// </summary>
        /// <param name="types">The list of generic types.</param>
        /// <returns>Returns a new type reference.</returns>
        public SchemaTypeNativeGeneric MakeGenericType(params ISchemaType[] types)
        {
            return new SchemaTypeNativeGeneric(schemaType, types);
        }
        
        /// <summary>
        /// Returns if this type matches the type information.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>Returns true, if the type matches.</returns>
        public bool Is<T>()
        {
            if (schemaType is SchemaTypeNative schemaTypeNative)
            {
                return schemaTypeNative.Type == typeof(T);
            }
            return false;
        }
    }

    /// <summary>
    /// Creates a new type info from the given C# type.
    /// </summary>
    /// <param name="type">The type to create.</param>
    /// <returns>Returns the type information instance.</returns>
    public static ISchemaType From(Type type) => new SchemaTypeNative(type);
    
    /// <summary>
    /// Creates a new type info from the given C# type.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <returns>Returns the type information instance.</returns>
    public static ISchemaType From<T>() => new SchemaTypeNative(typeof(T));
    
    /// <summary>
    /// Gets the <see cref="object"/> type.
    /// </summary>
    public static ISchemaType Object { get; } = From<object>();
    
    /// <summary>
    /// Gets the <see cref="int"/> type.
    /// </summary>
    public static ISchemaType Integer { get; } = From<int>();
    
    /// <summary>
    /// Gets the <see cref="float"/> type.
    /// </summary>
    public static ISchemaType Single { get; } = From<float>();
    
    /// <summary>
    /// Gets the <see cref="string"/> type.
    /// </summary>
    public static ISchemaType String { get; } = From<string>();
    
    /// <summary>
    /// Gets the <see cref="bool"/> type.
    /// </summary>
    public static ISchemaType Boolean { get; } = From<bool>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector2"/> type.
    /// </summary>
    public static ISchemaType Vector2 { get; } = From<Vector2>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector3"/> type.
    /// </summary>
    public static ISchemaType Vector3 { get; } = From<Vector3>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector4"/> type.
    /// </summary>
    public static ISchemaType Vector4 { get; } = From<Vector4>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Quaternion"/> type.
    /// </summary>
    public static ISchemaType Quaternion { get; } = From<Quaternion>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Matrix4x4"/> type.
    /// </summary>
    public static ISchemaType Matrix4x4 { get; } = From<Matrix4x4>();
    
    /// <summary>
    /// Gets the <see cref="Dictionary"/> type.
    /// </summary>
    public static ISchemaType Dictionary { get; } = From(typeof(Dictionary<,>));
}