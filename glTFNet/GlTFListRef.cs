using System.Collections;
using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a list of glTF model instances.
/// </summary>
/// <param name="context">The context this glTF was loaded from.</param>
/// <param name="source">The glTF list items.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public readonly struct GlTFListRef<T>(IGlTFContext context, IList<T> source) : IReadOnlyList<GlTFRef<T>>
{
    /// <inheritdoc />
    public IEnumerator<GlTFRef<T>> GetEnumerator()
    {
        var context1 = context;
        return source.Select((item, i) => new GlTFRef<T>(context1, item) { Index = i }).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => source.Count;

    /// <inheritdoc />
    public GlTFRef<T> this[int index] => new(context, source[index]) { Index = index };

    /// <summary>
    /// Gets an empty list reference.
    /// </summary>
    public static GlTFListRef<T> Empty { get; } = new(null!, []);
}