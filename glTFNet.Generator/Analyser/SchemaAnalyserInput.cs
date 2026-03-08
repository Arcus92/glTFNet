using glTFNet.Generator.Schema;

namespace glTFNet.Generator.Analyser;

/// <summary>
/// An input schema for the <see cref="SchemaAnalyser"/>.
/// </summary>
public class SchemaAnalyserInput
{
    /// <summary>
    /// Gets the schema to analyze.
    /// </summary>
    public required JsonSchema Schema { get; set; }
    
    /// <summary>
    /// Gets the destination namespace of the schema file.
    /// </summary>
    public required string Namespace { get; set; }
}