namespace glTFNet.Models.Extensions.KhrMaterialsVariants;

/// <summary>
/// glTF extension that defines a material variations for mesh primitives
/// </summary>
[Serializable]
public class GlTFKHRMaterialsVariants : GlTFProperty
{
    public required List<GlTFChildOfRootProperty> Variants { get; set; }
}