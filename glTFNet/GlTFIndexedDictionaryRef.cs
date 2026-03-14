using System.Collections;
using glTFNet.IO;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a dictionary of glTF model instances by a key-to-index dictionary and a source list.
/// </summary>
/// <param name="source">The glTF root list items.</param>
/// <param name="dictionary">The dictionary of indexes referencing items from the <paramref name="source"/> list.</param>
/// <param name="loader">The loader this GlTF was loaded from.</param>
/// <typeparam name="TKey">The dictionary index type.</typeparam>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public readonly struct GlTFIndexedDictionaryRef<TKey, T>(IList<T> source, IDictionary<TKey, int> dictionary, GlTFLoader loader) : IReadOnlyDictionary<TKey, GlTFRef<T>> where TKey : notnull
{
    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, GlTFRef<T>>> GetEnumerator()
    {
        var list1 = source;
        var loader1 = loader;
        return dictionary.Select(kvp => new KeyValuePair<TKey, GlTFRef<T>>(kvp.Key, new GlTFRef<T>(list1[kvp.Value], loader1) { Index = kvp.Value })).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => dictionary.Count;
    
    /// <inheritdoc />
    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    /// <inheritdoc />
    public bool TryGetValue(TKey key, out GlTFRef<T> value)
    {
        if (!dictionary.TryGetValue(key, out var index))
        {
            value = default;
            return false;
        }

        value = new GlTFRef<T>(source[index], loader) { Index = index };
        return true;
    }

    /// <inheritdoc />
    public GlTFRef<T> this[TKey key]
    {
        get
        {
            var index = dictionary[key];
            return new GlTFRef<T>(source[index], loader) { Index = index };
        }
    }

    /// <inheritdoc />
    public IEnumerable<TKey> Keys => dictionary.Keys;

    /// <inheritdoc />
    public IEnumerable<GlTFRef<T>> Values
    {
        get
        {
            var list1 = source;
            var loader1 = loader;
            return dictionary.Values.Select(index => new GlTFRef<T>(list1[index], loader1) { Index = index });
        }
    }
    
    /// <summary>
    /// Gets an empty dictionary reference.
    /// </summary>
    public static GlTFIndexedDictionaryRef<TKey, T> Empty { get; } = new([], new Dictionary<TKey, int>(), null!);
}