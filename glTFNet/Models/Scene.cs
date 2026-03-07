namespace glTFNet.Models
{
    /// <summary>
    /// The root nodes of a scene.
    /// </summary>
    [System.Serializable]
    public class Scene : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// The indices of each root node.
        /// </summary>
        public System.Int32[]? Nodes { get; set; }
    }
}