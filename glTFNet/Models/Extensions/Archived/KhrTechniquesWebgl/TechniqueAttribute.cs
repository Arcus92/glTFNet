namespace glTFNet.Models.Extensions.Archived.KhrTechniquesWebgl;

/// <summary>
/// An attribute input to a technique and the corresponding semantic.
/// </summary>
[Serializable]
public class TechniqueAttribute : glTFNet.Models.GlTFProperty
{
    /// <summary>
    /// Identifies a mesh attribute semantic. Attribute semantics include `"POSITION"`, `"NORMAL"`, `"TEXCOORD"`, `"COLOR"`, `"JOINT"`, and `"WEIGHT"`.  `"TEXCOORD"` and `"COLOR"` attribute semantic property names must be of the form `[semantic]_[set_index]`, e.g., `"TEXCOORD_0"`, `"TEXCOORD_1"`, `"COLOR_1"`, etc.  For forward-compatibility, application-specific semantics must start with an underscore, e.g., `"_SIMULATION_TIME"`.
    /// </summary>
    public required string Semantic { get; set; }
}