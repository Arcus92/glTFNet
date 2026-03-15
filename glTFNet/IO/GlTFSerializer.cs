using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using glTFNet.Specifications;
using JetBrains.Annotations;

namespace glTFNet.IO;

/// <summary>
/// Base class to handle glTF JSON serialization.
/// </summary>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public class GlTFSerializer
{
    /// <summary>
    /// A list of all serializers loaded.
    /// </summary>
    private readonly List<JsonSerializerContext> _serializerContexts = [ GlTFSerializerContext.Default ];

    /// <summary>
    /// Adds a new serializer context for extension serialization.
    /// </summary>
    /// <remarks>
    /// This is necessary when using external extensions via <see cref="GlTFRefExtensions.TryGetExtension"/>.
    /// </remarks>
    /// <param name="serializerContext">The serializer context to regierst.</param>
    public void AddSerializerContext(JsonSerializerContext serializerContext)
    {
        if (_serializerContexts.Contains(serializerContext))
        {
            return;
        }
        _serializerContexts.Add(serializerContext);
    }

    /// <summary>
    /// Gets the <see cref="JsonTypeInfo"/> from the given type.
    /// </summary>
    /// <remarks>
    /// Use <see cref="AddSerializerContext"/> to add support for external extensions.
    /// </remarks>
    /// <typeparam name="T">The type to get the type info from.</typeparam>
    /// <returns>Returns the type info.</returns>
    public JsonTypeInfo<T>? GetTypeInfo<T>()
    {
        foreach (var serializerContext in _serializerContexts)
        {
            if (serializerContext.GetTypeInfo(typeof(T)) is JsonTypeInfo<T> typeInfo)
            {
                return typeInfo;
            }
        }

        return null;
    }
}