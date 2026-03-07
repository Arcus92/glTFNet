using System.Numerics;

namespace glTFNet.Converters;

/// <summary>
/// The JSON converter for <see cref="Matrix4x4"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public class Matrix4x4Converter : VectorConverter<Matrix4x4>
{
    /// <inheritdoc />
    protected override int Size => 4;
    
    /// <inheritdoc />
    protected override float GetComponent(ref Matrix4x4 array, int index)
    {
        var row = index / 4;
        var col = index % 4;
        return array[row, col];
    }
    
    /// <inheritdoc />
    protected override void SetComponent(ref Matrix4x4 array, int index, float value)
    {
        var row = index / 4;
        var col = index % 4;
        array[row, col] = value;
    }
}