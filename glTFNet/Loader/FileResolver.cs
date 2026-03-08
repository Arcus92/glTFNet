using System.Diagnostics.CodeAnalysis;

namespace glTFNet.Loader;

/// <summary>
/// A <see cref="IResourceResolver"/> looking for resources relative to a directory.
/// </summary>
/// <param name="root">The root directory to load files from.</param>
public class FileResolver(string root) : IResourceResolver
{
    /// <summary>
    /// Gets the root path.
    /// </summary>
    public string Root { get; } = root;

    /// <inheritdoc />
    public bool TryResolve(string? uri, [MaybeNullWhen(false)] out Stream stream)
    {
        if (uri is null)
        {
            stream = null;
            return false;
        }
        
        var path = Path.Combine(Root, uri);
        if (!File.Exists(path))
        {
            stream = null;
            return false;
        }

        stream = File.OpenRead(path);
        return true;
    }
}