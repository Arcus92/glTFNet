namespace glTFNet.Models.Extensions.KhrMaterialsVariants;

[Serializable]
public class MeshPrimitiveKHRMaterialsVariants : GlTFProperty
{
    /// <summary>
    /// An array of object values that associate an indexed material to a set of variants.
    /// </summary>
    public required List<GlTFProperty> Mappings { get; set; }
}