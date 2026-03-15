using System.Collections;
using glTFNet.IO.Interfaces;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a list of glTF model instances.
/// </summary>
/// <param name="context">The context this glTF was loaded from.</param>
/// <param name="source">The glTF list items.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
public readonly struct GltfListRef<T>(IGltfContext context, IList<T> source) : IReadOnlyList<GltfRef<T>>
{
    /// <inheritdoc />
    public IEnumerator<GltfRef<T>> GetEnumerator()
    {
        var context1 = context;
        return source.Select((item, i) => new GltfRef<T>(context1, item) { Index = i }).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => source.Count;

    /// <inheritdoc />
    public GltfRef<T> this[int index] => new(context, source[index]) { Index = index };

    /// <summary>
    /// Gets an empty list reference.
    /// </summary>
    public static GltfListRef<T> Empty { get; } = new(null!, []);
}