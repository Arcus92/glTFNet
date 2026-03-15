using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet.IO;

/// <summary>
/// Defines a node for a context chain. Some glTF models don't reference the root <see cref="Gltf"/> instance but
/// rather another sub-layer like <see cref="Animation"/>.
/// This node element can be used as context to resolve the sub-layer if necessary.
/// </summary>
/// <param name="data">The glTF model instance to reference.</param>
/// <param name="parentContext">The parent context for this chained node.</param>
/// <typeparam name="TData">The glTF model type to reference.</typeparam>
[PublicAPI]
public class GltfNodeContext<TData>(TData data, IGltfContext? parentContext = null) : IGltfContext
{
    /// <summary>
    /// Gets the current glTG node instance.
    /// </summary>
    public TData Data { get; } = data;

    /// <inheritdoc />
    public T Parent<T>()
    {
        if (Data is T data)
        {
            return data;
        }

        if (parentContext is not null)
        {
            return parentContext.Parent<T>();
        }
        
        throw new InvalidOperationException($"The glTF model does not contain a parent of type {typeof(T)}.");
    }

    /// <inheritdoc />
    public T As<T>()
    {
        if (this is T result)
        {
            return result;
        }
        
        if (parentContext is not null)
        {
            return parentContext.As<T>();
        }
        
        throw new InvalidOperationException($"The context type {typeof(T)} could not be resolved.");
    }
}