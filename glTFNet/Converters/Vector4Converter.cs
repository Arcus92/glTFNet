using System.Numerics;

namespace glTFNet.Converters;

/// <summary>
/// The JSON converter for <see cref="Vector4"/>.
/// </summary>
public class Vector4Converter : VectorConverter<Vector4>
{
    /// <inheritdoc />
    protected override int Size => 4;

    /// <inheritdoc />
    protected override float GetComponent(ref Vector4 array, int index)
    {
        return array[index];
    }
    
    /// <inheritdoc />
    protected override void SetComponent(ref Vector4 array, int index, float value)
    {
        array[index] = value;
    }
}