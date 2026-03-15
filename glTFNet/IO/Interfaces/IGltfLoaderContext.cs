namespace glTFNet.IO.Interfaces;

/// <summary>
/// The context for a <see cref="GltfRef{T}"/> containing providing access to the loader.
/// </summary>
public interface IGltfLoaderContext
{
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
}