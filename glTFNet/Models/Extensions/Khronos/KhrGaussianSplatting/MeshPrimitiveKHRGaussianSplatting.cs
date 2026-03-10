namespace glTFNet.Models.Extensions.Khronos.KhrGaussianSplatting;

/// <summary>
/// Data defining a 3D Gaussian Splat primitive.
/// </summary>
[Serializable]
public class MeshPrimitiveKHRGaussianSplatting : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// Property specifying parameters regarding the kernel used to generate the Gaussians.
    /// </summary>
    public required string Kernel { get; set; }

    /// <summary>
    /// Property specifying the color space of the spherical harmonics.
    /// </summary>
    public required string ColorSpace { get; set; }

    /// <summary>
    /// Optional property specifying how to project the Gaussians to achieve a perspective correct value. This property defaults to perspective.
    /// </summary>
    public string? Projection { get; set; }

    /// <inheritdoc cref="Projection"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public string ProjectionOrDefault => Projection ?? "perspective";

    /// <summary>
    /// Optional property specifying how to sort the Gaussians during rendering. This property defaults to cameraDistance.
    /// </summary>
    public string? SortingMethod { get; set; }

    /// <inheritdoc cref="SortingMethod"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public string SortingMethodOrDefault => SortingMethod ?? "cameraDistance";
}