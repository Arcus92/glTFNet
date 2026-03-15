namespace glTFNet.IO.Interfaces;

/// <summary>
/// The context for a <see cref="GltfRef{T}"/> containing providing access to the current hierarchy.
/// </summary>
public interface IGltfContext
{
    /// <summary>
    /// Gets the parent glTF element of the given type.
    /// </summary>
    /// <typeparam name="T">The glTF model type to get.</typeparam>
    /// <returns>Returns the parent element.</returns>
    T Parent<T>();

    /// <summary>
    /// Converts the current context into another context type.
    /// </summary>
    /// <remarks>
    /// Throws an exception if the context type could not be resolved.
    /// </remarks>
    /// <typeparam name="T">The context type.</typeparam>
    /// <returns>Returns the context type.</returns>
    T As<T>();
}