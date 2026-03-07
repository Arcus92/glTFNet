namespace glTFNet.Models
{
    /// <summary>
    /// A node in the node hierarchy.  When the node contains `skin`, all `mesh.primitives` **MUST** contain `JOINTS_0` and `WEIGHTS_0` attributes.  A node **MAY** have either a `matrix` or any combination of `translation`/`rotation`/`scale` (TRS) properties. TRS properties are converted to matrices and postmultiplied in the `T * R * S` order to compose the transformation matrix; first the scale is applied to the vertices, then the rotation, and then the translation. If none are provided, the transform is the identity. When a node is targeted for animation (referenced by an animation.channel.target), `matrix` **MUST NOT** be present.
    /// </summary>
    [Serializable]
    public class Node : GlTFChildOfRootProperty
    {
        /// <summary>
        /// The index of the camera referenced by this node.
        /// </summary>
        public int? Camera { get; set; }

        /// <summary>
        /// The indices of this node's children.
        /// </summary>
        public List<int>? Children { get; set; }

        /// <summary>
        /// The index of the skin referenced by this node. When a skin is referenced by a node within a scene, all joints used by the skin **MUST** belong to the same scene. When defined, `mesh` **MUST** also be defined.
        /// </summary>
        public int? Skin { get; set; }

        /// <summary>
        /// A floating-point 4x4 transformation matrix stored in column-major order.
        /// </summary>
        public System.Numerics.Matrix4x4? Matrix { get; set; }

        /// <inheritdoc cref="Matrix"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Numerics.Matrix4x4 MatrixOrDefault => Matrix ?? new(1F, 0F, 0F, 0F, 0F, 1F, 0F, 0F, 0F, 0F, 1F, 0F, 0F, 0F, 0F, 1F);

        /// <summary>
        /// The index of the mesh in this node.
        /// </summary>
        public int? Mesh { get; set; }

        /// <summary>
        /// The node's unit quaternion rotation in the order (x, y, z, w), where w is the scalar.
        /// </summary>
        public System.Numerics.Quaternion? Rotation { get; set; }

        /// <inheritdoc cref="Rotation"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Numerics.Quaternion RotationOrDefault => Rotation ?? new(0F, 0F, 0F, 1F);

        /// <summary>
        /// The node's non-uniform scale, given as the scaling factors along the x, y, and z axes.
        /// </summary>
        public System.Numerics.Vector3? Scale { get; set; }

        /// <inheritdoc cref="Scale"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Numerics.Vector3 ScaleOrDefault => Scale ?? new(1F, 1F, 1F);

        /// <summary>
        /// The node's translation along the x, y, and z axes.
        /// </summary>
        public System.Numerics.Vector3? Translation { get; set; }

        /// <inheritdoc cref="Translation"/>
        [System.Text.Json.Serialization.JsonIgnore]
        public System.Numerics.Vector3 TranslationOrDefault => Translation ?? new(0F, 0F, 0F);

        /// <summary>
        /// The weights of the instantiated morph target. The number of array elements **MUST** match the number of morph targets of the referenced mesh. When defined, `mesh` **MUST** also be defined.
        /// </summary>
        public List<float>? Weights { get; set; }
    }
}