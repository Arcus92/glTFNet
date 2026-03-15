namespace glTFNet.IO;

/// <summary>
/// Resolves a resource uri and loads the resource stream.
/// </summary>
public interface IResourceResolver
{
    /// <summary>
    /// Resolves the resource uri and opens the resource stream.
    /// </summary>
    /// <param name="uri">The resource uri to open.</param>
    /// <returns>Returns the readable stream of the resource. Returns null, it the stream could not be resolved.</returns>
    Task<Stream?> Resolve(string? uri);
}