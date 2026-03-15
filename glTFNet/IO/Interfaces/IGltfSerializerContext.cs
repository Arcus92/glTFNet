using System.Text.Json.Serialization.Metadata;

namespace glTFNet.IO.Interfaces;

/// <summary>
/// The context for a <see cref="GltfRef{T}"/> containing providing access to the JSON serializer.
/// </summary>
public interface IGltfSerializerContext
{
    /// <summary>
    /// Gets the <see cref="JsonTypeInfo{T}"/> from the given type.
    /// </summary>
    /// <typeparam name="T">The type to get the type info from.</typeparam>
    /// <returns>Returns the type info.</returns>
    JsonTypeInfo<T>? GetTypeInfo<T>();
}