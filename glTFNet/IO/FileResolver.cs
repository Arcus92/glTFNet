namespace glTFNet.IO;

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
    public Task<Stream?> Resolve(string? uri)
    {
        if (uri is null)
        {
            return Task.FromResult<Stream?>(null);
        }
        
        var path = Path.Combine(Root, uri);
        if (!File.Exists(path))
        {
            return Task.FromResult<Stream?>(null);
        }

        return Task.FromResult<Stream?>(File.OpenRead(path));
    }
}