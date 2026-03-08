using System.Diagnostics.CodeAnalysis;

namespace glTFNet.Loader;

/// <summary>
/// Resolves a resource uri and loads the resource stream.
/// </summary>
public interface IResourceResolver
{
    /// <summary>
    /// Resolves the resource uri and opens the resource stream.
    /// </summary>
    /// <param name="uri">The resource uri to open.</param>
    /// <param name="stream">Returns the readable stream of the resource.</param>
    /// <returns>Returns true, if the resource was found and the stream was opened.</returns>
    bool TryResolve(string? uri, [MaybeNullWhen(false)] out Stream stream);
}