namespace glTFNet.Models
{
    /// <summary>
    /// A perspective camera containing properties to create a perspective projection matrix.
    /// </summary>
    [System.Serializable]
    public class CameraPerspective : GlTFProperty
    {
        /// <summary>
        /// The floating-point aspect ratio of the field of view. When undefined, the aspect ratio of the rendering viewport **MUST** be used.
        /// </summary>
        public System.Single? AspectRatio { get; set; }

        /// <summary>
        /// The floating-point vertical field of view in radians. This value **SHOULD** be less than π.
        /// </summary>
        public required System.Single Yfov { get; set; }

        /// <summary>
        /// The floating-point distance to the far clipping plane. When defined, `zfar` **MUST** be greater than `znear`. If `zfar` is undefined, client implementations **SHOULD** use infinite projection matrix.
        /// </summary>
        public System.Single? Zfar { get; set; }

        /// <summary>
        /// The floating-point distance to the near clipping plane.
        /// </summary>
        public required System.Single Znear { get; set; }
    }
}