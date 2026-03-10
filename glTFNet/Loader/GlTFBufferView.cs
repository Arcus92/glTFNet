using System.Numerics;
using System.Runtime.CompilerServices;
using glTFNet.Models;
using JetBrains.Annotations;

namespace glTFNet.Loader;

/// <summary>
/// Gets a loaded buffer view.
/// </summary>
/// <param name="data">The initial data.</param>
[PublicAPI]
// ReSharper disable once InconsistentNaming
public class GlTFBufferView(byte[] data, BufferView bufferView)
{
    /// <summary>
    /// Gets the binary data.
    /// </summary>
    public byte[] Data { get; } = data;

    /// <summary>
    /// Gets the buffer view model.
    /// </summary>
    public BufferView BufferView { get; } = bufferView;

    /// <summary>
    /// Reads the values for the given accessor.
    /// </summary>
    /// <param name="accessor">The accessor to read.</param>
    /// <returns>Returns the values.</returns>
    public Array Read(Accessor accessor)
    {
        return accessor.Type switch
        {
            AccessorType.Scalar => accessor.ComponentType switch
            {
                AccessorComponentType.Byte => Read<sbyte>(accessor),
                AccessorComponentType.UnsignedByte => Read<byte>(accessor),
                AccessorComponentType.Short => Read<short>(accessor),
                AccessorComponentType.UnsignedShort => Read<ushort>(accessor),
                AccessorComponentType.UnsignedInt => Read<uint>(accessor),
                AccessorComponentType.Float => Read<float>(accessor),
                _ => throw new ArgumentOutOfRangeException()
            },
            AccessorType.Vec2 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector2>(accessor),
            AccessorType.Vec3 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector3>(accessor), 
            AccessorType.Vec4 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector4>(accessor), 
            AccessorType.Mat4 when accessor.ComponentType == AccessorComponentType.Float => Read<Matrix4x4>(accessor),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Reads the values for the given accessor.
    /// </summary>
    /// <param name="accessor">The accessor to read.</param>
    /// <typeparam name="T">The value type.</typeparam>
    /// <returns>Returns the values.</returns>
    public unsafe T[] Read<T>(Accessor accessor) where T : struct
    {
        var structSize = Unsafe.SizeOf<T>();
        var componentSize = GetComponentSize(accessor.ComponentType);
        var componentCount = GetComponentCount(accessor.Type);
        var size = componentSize * componentCount;
        if (structSize != size)
        {
            throw new ArgumentException($"The accessor reads {size} bytes, but {nameof(T)} requires {structSize} bytes.");
        }

        var count = accessor.Count;
        var offset = accessor.ByteOffsetOrDefault;
        var stride = BufferView.ByteStride ?? size;
        
        // Reads the data
        var result = new T[count];
        fixed (byte* ptr = &Data[offset])
        {
            for (var i = 0; i < count; i++)
            {
                result[i] = Unsafe.Read<T>(ptr + i * stride);
            }
        }
        return result;
    }
    
    /// <summary>
    /// Gets the size of the component.
    /// </summary>
    /// <param name="componentType">The component type.</param>
    /// <returns>Returns the size of the component in bytes.</returns>
    private static int GetComponentSize(AccessorComponentType componentType)
    {
        return componentType switch
        {
            AccessorComponentType.Byte => 1,
            AccessorComponentType.UnsignedByte => 1,
            AccessorComponentType.Short => 2,
            AccessorComponentType.UnsignedShort => 2,
            AccessorComponentType.UnsignedInt => 4,
            AccessorComponentType.Float => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
        };
    }
    
    /// <summary>
    /// Gets the number of element per item.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Returns the element.</returns>
    private static int GetComponentCount(AccessorType type)
    {
        return type switch
        {
            AccessorType.Scalar => 1,
            AccessorType.Vec2 => 2,
            AccessorType.Vec3 => 3,
            AccessorType.Vec4 => 4,
            AccessorType.Mat2 => 4,
            AccessorType.Mat3 => 9,
            AccessorType.Mat4 => 16,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}