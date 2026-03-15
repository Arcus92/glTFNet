using System.Numerics;

namespace glTFNet.Specifications.Converters;

/// <summary>
/// The JSON converter for <see cref="Vector2"/>.
/// </summary>
public class Vector2Converter : VectorConverter<Vector2>
{
    /// <inheritdoc />
    protected override int Size => 2;

    /// <inheritdoc />
    protected override float GetComponent(ref Vector2 array, int index)
    {
        return array[index];
    }
    
    /// <inheritdoc />
    protected override void SetComponent(ref Vector2 array, int index, float value)
    {
        array[index] = value;
    }
}