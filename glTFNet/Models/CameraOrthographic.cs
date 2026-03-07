namespace glTFNet.Models
{
    /// <summary>
    /// An orthographic camera containing properties to create an orthographic projection matrix.
    /// </summary>
    [System.Serializable]
    public class CameraOrthographic : glTFNet.Models.GlTFProperty
    {
        /// <summary>
        /// The floating-point horizontal magnification of the view. This value **MUST NOT** be equal to zero. This value **SHOULD NOT** be negative.
        /// </summary>
        public required System.Single Xmag { get; set; }

        /// <summary>
        /// The floating-point vertical magnification of the view. This value **MUST NOT** be equal to zero. This value **SHOULD NOT** be negative.
        /// </summary>
        public required System.Single Ymag { get; set; }

        /// <summary>
        /// The floating-point distance to the far clipping plane. This value **MUST NOT** be equal to zero. `zfar` **MUST** be greater than `znear`.
        /// </summary>
        public required System.Single Zfar { get; set; }

        /// <summary>
        /// The floating-point distance to the near clipping plane.
        /// </summary>
        public required System.Single Znear { get; set; }
    }
}