using System.Collections;
using glTFNet.Loader;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a list of glTF model instances.
/// </summary>
/// <param name="source">The glTF list items.</param>
/// <param name="loader">The loader this GlTF was loaded from.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public readonly struct GlTFListRef<T>(IList<T> source, GlTFLoader loader) : IReadOnlyList<GlTFRef<T>>
{
    /// <inheritdoc />
    public IEnumerator<GlTFRef<T>> GetEnumerator()
    {
        var loader1 = loader;
        return source.Select((item, i) => new GlTFRef<T>(item, loader1) { Index = i }).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => source.Count;

    /// <inheritdoc />
    public GlTFRef<T> this[int index] => new(source[index], loader) { Index = index };

    /// <summary>
    /// Gets an empty list reference.
    /// </summary>
    public static GlTFListRef<T> Empty { get; } = new([], null!);
}