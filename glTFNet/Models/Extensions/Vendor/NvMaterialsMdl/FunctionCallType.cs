namespace glTFNet.Models.Extensions.Vendor.NvMaterialsMdl;

/// <summary>
/// MDL type describing either a built-in or user-defined type, or an array of a built-in or user-defined type.
/// </summary>
[Serializable]
public class FunctionCallType : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// The ID of the containing module.  This field **MUST NOT** be defined if a built-in type is specified.
    /// </summary>
    public int? Module { get; set; }

    /// <summary>
    /// The unqualified name of the type.
    /// </summary>
    public required string TypeName { get; set; }

    /// <summary>
    /// The array size. If this field is defined the type is considered to be a array.
    /// </summary>
    public int? ArraySize { get; set; }

    /// <summary>
    /// The name of the type modifier.
    /// </summary>
    public FunctionCallTypeModifier? Modifier { get; set; }
}