using System.Collections;
using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a list of glTF model instances by an index list and a source list.
/// </summary>
/// <param name="source">The glTF root list items.</param>
/// <param name="indices">The list of indexes referencing items from the <paramref name="source"/> list.</param>
/// <param name="loader">The loader this GlTF was loaded from.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public readonly struct GlTFIndexedListRef<T>(IList<T> source, IList<int> indices, GlTFLoader loader) : IReadOnlyList<GlTFRef<T>>
{
    /// <inheritdoc />
    public IEnumerator<GlTFRef<T>> GetEnumerator()
    {
        var loader1 = loader;
        var list1 = source;
        return indices.Select(index => new GlTFRef<T>(list1[index], loader1)
        {
            Index = index
        }).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => source.Count;

    /// <inheritdoc />
    public GlTFRef<T> this[int index]
    {
        get
        {
            var listIndex = indices[index];
            return new GlTFRef<T>(source[listIndex], loader) { Index = listIndex };
        }
    }

    /// <summary>
    /// Gets an empty list reference.
    /// </summary>
    public static GlTFIndexedListRef<T> Empty { get; } =  new([], [], null!);
}