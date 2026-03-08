namespace glTFNet.Models.Extensions.KhrNodeVisibility;

/// <summary>
/// glTF extension that defines node's visibility.
/// </summary>
[Serializable]
public class NodeKHRNodeVisibility : GlTFProperty
{
    /// <summary>
    /// Specifies whether the node is visible. A value of false means that the node and all its children are hidden.
    /// </summary>
    public bool? Visible { get; set; }

    /// <inheritdoc cref="Visible"/>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool VisibleOrDefault => Visible ?? true;
}