using System.Collections;
using glTFNet.IO.Interfaces;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// A wrapper for a dictionary of glTF model instances by a key-to-index dictionary and a source list.
/// </summary>
/// <param name="context">The context this glTF was loaded from.</param>
/// <param name="source">The glTF root list items.</param>
/// <param name="dictionary">The dictionary of indexes referencing items from the <paramref name="source"/> list.</param>
/// <typeparam name="TKey">The dictionary index type.</typeparam>
/// <typeparam name="T">The glTF model type.</typeparam>
[PublicAPI]
public readonly struct GltfIndexedDictionaryRef<TKey, T>(
    IGltfContext context,
    IList<T> source,
    IDictionary<TKey, int> dictionary) : IReadOnlyDictionary<TKey, GltfRef<T>> where TKey : notnull
{
    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, GltfRef<T>>> GetEnumerator()
    {
        var list1 = source;
        var context1 = context;
        return dictionary.Select(kvp => new KeyValuePair<TKey, GltfRef<T>>(kvp.Key, new GltfRef<T>(context1, list1[kvp.Value]) { Index = kvp.Value })).GetEnumerator();
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
    public bool TryGetValue(TKey key, out GltfRef<T> value)
    {
        if (!dictionary.TryGetValue(key, out var index))
        {
            value = default;
            return false;
        }

        value = new GltfRef<T>(context, source[index]) { Index = index };
        return true;
    }

    /// <inheritdoc />
    public GltfRef<T> this[TKey key]
    {
        get
        {
            var index = dictionary[key];
            return new GltfRef<T>(context, source[index]) { Index = index };
        }
    }

    /// <inheritdoc />
    public IEnumerable<TKey> Keys => dictionary.Keys;

    /// <inheritdoc />
    public IEnumerable<GltfRef<T>> Values
    {
        get
        {
            var list1 = source;
            var context1 = context;
            return dictionary.Values.Select(index => new GltfRef<T>(context1, list1[index]) { Index = index });
        }
    }
    
    /// <summary>
    /// Gets an empty dictionary reference.
    /// </summary>
    public static GltfIndexedDictionaryRef<TKey, T> Empty { get; } = new(null!, [], new Dictionary<TKey, int>());
}