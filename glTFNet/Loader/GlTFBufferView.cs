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
    /// Returns a new memory stream from this buffer.
    /// </summary>
    /// <returns>Returns the new memory stream.</returns>
    public Stream AsStream()
    {
        return new MemoryStream(Data);
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
        var elementSize = GetElementSize(accessor.Type, accessor.ComponentType);
        if (structSize != elementSize)
        {
            throw new ArgumentException($"The accessor reads {elementSize} bytes, but {nameof(T)} requires {structSize} bytes.");
        }

        var count = accessor.Count;
        var offset = accessor.ByteOffsetOrDefault;
        var stride = BufferView.ByteStride ?? elementSize;
        
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
    /// Reads the values for the given accessor.
    /// </summary>
    /// <remarks>
    /// Only Scalar is fully supported! Vec2, Vec3, Vec4 and Mat4x4 only support float values. Mat2x2 and Mat3x3
    /// are not supported.<br/>
    /// You can use <see cref="Read{T}"/> to pass a custom struct with the given length to bypass this limitation.
    /// </remarks>
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
                _ => throw new NotSupportedException($"Unsupported accessor type: {accessor.ComponentType}")
            },
            AccessorType.Vec2 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector2>(accessor),
            AccessorType.Vec3 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector3>(accessor), 
            AccessorType.Vec4 when accessor.ComponentType == AccessorComponentType.Float => Read<Vector4>(accessor), 
            AccessorType.Mat4 when accessor.ComponentType == AccessorComponentType.Float => Read<Matrix4x4>(accessor),
            _ => throw new NotSupportedException($"Unsupported accessor type and component type combination: {accessor.ComponentType}-{accessor.ComponentType}")
        };
    }

    /// <summary>
    /// Gets the size in bytes for the element defined by the accessor type and component type.
    /// </summary>
    /// <param name="type">The accessor defines how many components are used per element.</param>
    /// <param name="componentType">The component type defines how many bytes a single component uses.</param>
    /// <returns>Returns the size per element in bytes.</returns>
    public static int GetElementSize(AccessorType type, AccessorComponentType componentType)
    {
        var componentCount = GetComponentCount(type);
        var componentSize = GetComponentSize(componentType);
        return componentSize * componentCount;
    }
    
    /// <summary>
    /// Gets the size in bytes for the given component type.
    /// </summary>
    /// <param name="componentType">The component type defines how many bytes a single component uses.</param>
    /// <returns>Returns the size of the component in bytes.</returns>
    public static int GetComponentSize(AccessorComponentType componentType)
    {
        return componentType switch
        {
            AccessorComponentType.Byte => sizeof(sbyte),
            AccessorComponentType.UnsignedByte => sizeof(byte),
            AccessorComponentType.Short => sizeof(short),
            AccessorComponentType.UnsignedShort => sizeof(ushort),
            AccessorComponentType.UnsignedInt => sizeof(uint),
            AccessorComponentType.Float => sizeof(float),
            _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
        };
    }
    
    /// <summary>
    /// Gets the number of components per element.
    /// </summary>
    /// <param name="type">The accessor defines how many components are used per element.</param>
    /// <returns>Returns the element.</returns>
    public static int GetComponentCount(AccessorType type)
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