using System.Numerics;

namespace glTFNet.Converters;

/// <summary>
/// The JSON converter for <see cref="Vector3"/>.
/// </summary>
public class Vector3Converter : VectorConverter<Vector3>
{
    /// <inheritdoc />
    protected override int Size => 3;

    /// <inheritdoc />
    protected override float GetComponent(ref Vector3 array, int index)
    {
        return array[index];
    }
    
    /// <inheritdoc />
    protected override void SetComponent(ref Vector3 array, int index, float value)
    {
        array[index] = value;
    }
}