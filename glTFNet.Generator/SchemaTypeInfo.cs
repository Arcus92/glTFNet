using System.Numerics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace glTFNet.Generator;

/// <summary>
/// Defines the schema type information.
/// </summary>
public readonly record struct SchemaTypeInfo
{
    /// <summary>
    /// Creates a new type information instance.
    /// </summary>
    /// <param name="type">The type.</param>
    public SchemaTypeInfo(Type type)
    {
        Name = type.FullName!;
    }
    
    /// <summary>
    /// Creates a new type information instance.
    /// </summary>
    /// <param name="name">The type name.</param>
    public SchemaTypeInfo(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the C# name of the type.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates an array type information of this type.
    /// </summary>
    /// <returns>Returns the new array type.</returns>
    public SchemaTypeInfo AsArray()
    {
        return new SchemaTypeInfo($"{Name}[]");
    }
    
    /// <summary>
    /// Creates an nullable type information of this type.
    /// </summary>
    /// <returns>Returns the new nullable type.</returns>
    public SchemaTypeInfo AsNullable()
    {
        return new SchemaTypeInfo($"{Name}?");
    }

    /// <summary>
    /// Returns the type syntax.
    /// </summary>
    /// <returns>Returns the type syntax.</returns>
    public TypeSyntax AsTypeSyntax()
    {
        return SyntaxFactory.ParseTypeName(Name);
    }

    /// <summary>
    /// Returns if this type matches the type information.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <returns>Returns true, if the type matches.</returns>
    public bool Is<T>()
    {
        return this == From<T>();
    }
    
    #region Pre defined
    
    /// <summary>
    /// Creates a new type info from the given C# type.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <returns>Returns the type information instance.</returns>
    public static SchemaTypeInfo From<T>() => new(typeof(T));
    
    /// <summary>
    /// Gets the <see cref="object"/> type.
    /// </summary>
    public static SchemaTypeInfo Object { get; } = From<object>();
    
    /// <summary>
    /// Gets the <see cref="int"/> type.
    /// </summary>
    public static SchemaTypeInfo Integer { get; } = From<int>();
    
    /// <summary>
    /// Gets the <see cref="float"/> type.
    /// </summary>
    public static SchemaTypeInfo Single { get; } = From<float>();
    
    /// <summary>
    /// Gets the <see cref="string"/> type.
    /// </summary>
    public static SchemaTypeInfo String { get; } = From<string>();
    
    /// <summary>
    /// Gets the <see cref="bool"/> type.
    /// </summary>
    public static SchemaTypeInfo Boolean { get; } = From<bool>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector2"/> type.
    /// </summary>
    public static SchemaTypeInfo Vector2 { get; } = From<Vector2>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector3"/> type.
    /// </summary>
    public static SchemaTypeInfo Vector3 { get; } = From<Vector3>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Vector4"/> type.
    /// </summary>
    public static SchemaTypeInfo Vector4 { get; } = From<Vector4>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Quaternion"/> type.
    /// </summary>
    public static SchemaTypeInfo Quaternion { get; } = From<Quaternion>();
    
    /// <summary>
    /// Gets the <see cref="System.Numerics.Matrix4x4"/> type.
    /// </summary>
    public static SchemaTypeInfo Matrix4x4 { get; } = From<Matrix4x4>();
    
    #endregion Pre defined
}