using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/> with a <see cref="Accessor"/> reference.
/// </summary>
[PublicAPI]
public static class GltfRefAccessorExtensions
{
    /// <summary>
    /// Gets the buffer view of the glTF accessor.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <value>Returns the buffer view.</value>
    public static GltfRef<BufferView>? BufferView(this GltfRef<Accessor> instance)
    {
        var root = instance.Context.Parent<Gltf>();
        if (!instance.Data.BufferView.HasValue || root.BufferViews is null)
        {
            return null;
        }

        return instance.Ref(root.BufferViews, instance.Data.BufferView.Value);
    }

    /// <summary>
    /// Gets the buffer view of the glTF accessor.
    /// </summary>
    /// <param name="instance">The glTF reference.</param>
    /// <param name="result">Returns the buffer view if found.</param>
    /// <returns>Returns true, if the buffer view was found.</returns>
    public static bool HasBufferView(this GltfRef<Accessor> instance, out GltfRef<BufferView> result) =>
        instance.BufferView().TryGetValue(out result);

    /// <summary>
    /// Reads the data from the glTF accessor.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <remarks>
    /// Only Scalar is fully supported! Vec2, Vec3, Vec4 and Mat4x4 only support float values. Mat2x2 and Mat3x3
    /// are not supported.<br/>
    /// You can use <see cref="Read{T}"/> to pass a custom struct with the given length to bypass this limitation.
    /// </remarks>
    /// <returns>Returns the data.</returns>
    public static async Task<Array?> Read(this GltfRef<Accessor> instance)
    {
        var bufferView = instance.BufferView();
        if (!bufferView.HasValue)
        {
            return null;
        }

        // Loads the buffer view
        var loadedBufferView = await bufferView.Value.Open();
        return loadedBufferView?.Read(instance.Data);
    }

    /// <summary>
    /// Reads the data from the glTF accessor as the given type.
    /// </summary>
    /// <param name="instance">The glTF accessor reference.</param>
    /// <remarks>
    /// The component type must match the component size in the accessor.
    /// </remarks>
    /// <typeparam name="T">The component type to read.</typeparam>
    /// <returns>Returns the data.</returns>
    public static async Task<T[]?> Read<T>(this GltfRef<Accessor> instance) where T : struct
    {
        var bufferView = instance.BufferView();
        if (!bufferView.HasValue)
        {
            return null;
        }

        // Loads the buffer view
        var loadedBufferView = await bufferView.Value.Open();
        return loadedBufferView?.Read<T>(instance.Data);
    }
}