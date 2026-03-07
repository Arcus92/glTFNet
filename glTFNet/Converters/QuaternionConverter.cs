using System.Numerics;

namespace glTFNet.Converters;

/// <summary>
/// The JSON converter for <see cref="Quaternion"/>.
/// </summary>
public class QuaternionConverter : VectorConverter<Quaternion>
{
    /// <inheritdoc />
    protected override int Size => 4;

    /// <inheritdoc />
    protected override float GetComponent(ref Quaternion array, int index)
    {
        return array[index];
    }
    
    /// <inheritdoc />
    protected override void SetComponent(ref Quaternion array, int index, float value)
    {
        array[index] = value;
    }
}