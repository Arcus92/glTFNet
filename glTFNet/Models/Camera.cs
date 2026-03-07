namespace glTFNet.Models
{
    /// <summary>
    /// A camera's projection.  A node **MAY** reference a camera to apply a transform to place the camera in the scene.
    /// </summary>
    [System.Serializable]
    public class Camera : glTFNet.Models.GlTFChildOfRootProperty
    {
        /// <summary>
        /// An orthographic camera containing properties to create an orthographic projection matrix. This property **MUST NOT** be defined when `perspective` is defined.
        /// </summary>
        public glTFNet.Models.CameraOrthographic? Orthographic { get; set; }

        /// <summary>
        /// A perspective camera containing properties to create a perspective projection matrix. This property **MUST NOT** be defined when `orthographic` is defined.
        /// </summary>
        public glTFNet.Models.CameraPerspective? Perspective { get; set; }

        /// <summary>
        /// Specifies if the camera uses a perspective or orthographic projection.  Based on this, either the camera's `perspective` or `orthographic` property **MUST** be defined.
        /// </summary>
        public required glTFNet.Models.CameraType Type { get; set; }
    }
}