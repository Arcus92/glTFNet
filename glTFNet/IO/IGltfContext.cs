using System.Text.Json.Serialization.Metadata;

namespace glTFNet.IO;

/// <summary>
/// The context for a <see cref="GltfRef{T}"/> containing providing access to the loader.
/// </summary>
public interface IGltfContext
{
    /// <summary>
    /// Gets the parent glTF element of the given type.
    /// </summary>
    /// <typeparam name="T">The glTF model type to get.</typeparam>
    /// <returns>Returns the parent element.</returns>
    T Parent<T>();

    /// <summary>
    /// Tries to resolve an external resource as stream.
    /// </summary>
    /// <param name="uri">The uri to load.</param>
    /// <returns>Returns the readable stream of the resource. Returns null, it the stream could not be resolved.</returns>
    Task<Stream?> OpenUriAsStream(string? uri);

    /// <summary>
    /// Tries to resolve an external binary buffer.
    /// </summary>
    /// <param name="uri">The uri to load.</param>
    /// <returns>Returns the requested resource as binary buffer.</returns>
    Task<GltfBuffer?> OpenUriAsBuffer(string? uri);

    /// <summary>
    /// Gets the <see cref="JsonTypeInfo"/> from the given type.
    /// </summary>
    /// <typeparam name="T">The type to get the type info from.</typeparam>
    /// <returns>Returns the type info.</returns>
    JsonTypeInfo<T>? GetTypeInfo<T>();
}