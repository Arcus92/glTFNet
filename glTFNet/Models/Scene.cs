namespace glTFNet.Models
{
    /// <summary>
    /// The root nodes of a scene.
    /// </summary>
    [Serializable]
    public class Scene : GlTFChildOfRootProperty
    {
        /// <summary>
        /// The indices of each root node.
        /// </summary>
        public List<int>? Nodes { get; set; }
    }
}