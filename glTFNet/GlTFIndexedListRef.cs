using System.Collections;
using glTFNet.IO;

namespace glTFNet;

/// <summary>
/// A wrapper for a list of glTF model instances by an index list and a source list.
/// </summary>
/// <param name="context">The context this glTF was loaded from.</param>
/// <param name="source">The glTF root list items.</param>
/// <param name="indices">The list of indexes referencing items from the <paramref name="source"/> list.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
// ReSharper disable once InconsistentNaming
public readonly struct GlTFIndexedListRef<T>(IGlTFContext context, IList<T> source, IList<int> indices) : IReadOnlyList<GlTFRef<T>>
{
    /// <inheritdoc />
    public IEnumerator<GlTFRef<T>> GetEnumerator()
    {
        var context1 = context;
        var list1 = source;
        return indices.Select(index => new GlTFRef<T>(context1, list1[index])
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
            return new GlTFRef<T>(context, source[listIndex]) { Index = listIndex };
        }
    }

    /// <summary>
    /// Gets an empty list reference.
    /// </summary>
    public static GlTFIndexedListRef<T> Empty { get; } =  new(null!, [], []);
}