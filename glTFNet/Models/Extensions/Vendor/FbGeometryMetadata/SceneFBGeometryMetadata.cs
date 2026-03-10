namespace glTFNet.Models.Extensions.Vendor.FbGeometryMetadata;

[Serializable]
public class SceneFBGeometryMetadata : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The number of distinct vertices recursively contained in this scene.
    /// </summary>
    public float? VertexCount { get; set; }

    /// <inheritdoc cref="VertexCount"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float VertexCountOrDefault => VertexCount ?? 0F;

    /// <summary>
    /// The number of distinct primitives recursively contained in this scene.
    /// </summary>
    public float? PrimitiveCount { get; set; }

    /// <inheritdoc cref="PrimitiveCount"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public float PrimitiveCountOrDefault => PrimitiveCount ?? 0F;

    /// <summary>
    /// The bounding box of this scene, in static geometry scene-space coordinates.
    /// </summary>
    public SceneBounds? SceneBounds { get; set; }
}