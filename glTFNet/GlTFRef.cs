using glTFNet.IO;

namespace glTFNet;

/// <summary>
/// A wrapper to reference a loaded glTF data instance.
/// The wrapper adds easier access to referenced sub models via internal ids.
/// </summary>
/// <param name="context">The context this glTF was loaded from.</param>
/// <param name="data">The glTF model data.</param>
/// <typeparam name="T">The glTF model type.</typeparam>
// ReSharper disable once InconsistentNaming
public readonly struct GlTFRef<T>(IGlTFContext context, T data)
{
    /// <summary>
    /// Gets the underlying glTF model.
    /// </summary>
    public T Data { get; } = data;

    /// <summary>
    /// Gets the index of this reference in the array of the root glTF instance. This can be used as unique id.
    /// </summary>
    public int Index { get; init; } = -1;

    /// <summary>
    /// Gets the glTF context.
    /// </summary>
    internal IGlTFContext Context { get; } = context;

    /// <summary>
    /// Creates a glTF reference.
    /// </summary>
    /// <param name="instance">The glTF model to reference.</param>
    /// <typeparam name="TNew">The glTF instance type.</typeparam>
    /// <returns>Returns the referenced glTF model.</returns>
    internal GlTFRef<TNew> Ref<TNew>(TNew instance)
    {
        return new GlTFRef<TNew>(Context, instance);
    }
    
    /// <summary>
    /// Creates a glTF reference to a root array item.
    /// </summary>
    /// <param name="source">The root glTF list to reference from.</param>
    /// <param name="index">The list index to reference from.</param>
    /// <typeparam name="TNew">The glTF instance type.</typeparam>
    /// <returns>Returns the referenced glTF model.</returns>
    internal GlTFRef<TNew> Ref<TNew>(IList<TNew> source, int index)
    {
        var instance = source[index];
        return new GlTFRef<TNew>(Context, instance)
        {
            Index = index
        };
    }
    
    /// <summary>
    /// Creates a glTF reference list.
    /// </summary>
    /// <param name="list">The glTF list to reference.</param>
    /// <typeparam name="TNew">The glTF instance type.</typeparam>
    /// <returns>Returns the list of referenced glTF model.</returns>
    internal GlTFListRef<TNew> RefList<TNew>(IList<TNew> list)
    {
        return new GlTFListRef<TNew>(Context, list);
    }
    
    /// <summary>
    /// Creates a glTF reference list by using an index list to reference a root array.
    /// </summary>
    /// <param name="source">The root glTF list to reference from.</param>
    /// <param name="indices">The index list to reference from.</param>
    /// <typeparam name="TNew">The glTF instance type.</typeparam>
    /// <returns>Returns the list of referenced glTF model.</returns>
    internal GlTFIndexedListRef<TNew> RefIndexedList<TNew>(IList<TNew> source, IList<int> indices)
    {
        return new GlTFIndexedListRef<TNew>(Context, source, indices);
    }
    
    /// <summary>
    /// Creates a glTF reference dictionary by using a key-to-index dictionary to reference a root array.
    /// </summary>
    /// <param name="source">The root glTF list to reference from.</param>
    /// <param name="dictionary">The key-to-index dictionary to reference from.</param>
    /// <typeparam name="TKey">The dictionary key.</typeparam>
    /// <typeparam name="TNew">The glTF instance type.</typeparam>
    /// <returns>Returns the dictionary of referenced glTF model.</returns>
    internal GlTFIndexedDictionaryRef<TKey, TNew> RefIndexedDictionary<TKey, TNew>(IList<TNew> source, IDictionary<TKey, int> dictionary) where TKey : notnull
    {
        return new GlTFIndexedDictionaryRef<TKey, TNew>(Context, source, dictionary);
    }
    
    /// <summary>
    /// Casts the GlTF reference to the model.
    /// </summary>
    /// <param name="gltfRef">The reference struct.</param>
    /// <returns>Returns the model.</returns>
    public static implicit operator T(GlTFRef<T> gltfRef) => gltfRef.Data;
}