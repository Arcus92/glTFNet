namespace glTFNet.Models.Extensions.KhrMeshoptCompression;

/// <summary>
/// Compressed data for bufferView.
/// </summary>
[Serializable]
public class BufferKHRMeshoptCompression : GlTFProperty
{
    /// <summary>
    /// Set to true to indicate that the buffer is only referenced by bufferViews that have KHR_meshopt_compression extension and as such doesn't need to be loaded.
    /// </summary>
    public bool? Fallback { get; set; }

    /// <inheritdoc cref="Fallback"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool FallbackOrDefault => Fallback ?? false;
}