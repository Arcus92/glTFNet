namespace glTFNet.Models.Extensions.Vendor.ExtMeshoptCompression;

/// <summary>
/// Compressed data for bufferView.
/// </summary>
[Serializable]
public class BufferEXTMeshoptCompression : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// Set to true to indicate that the buffer is only referenced by bufferViews that have EXT_meshopt_compression extension and as such doesn't need to be loaded.
    /// </summary>
    public bool? Fallback { get; set; }

    /// <inheritdoc cref="Fallback"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool FallbackOrDefault => Fallback ?? false;
}