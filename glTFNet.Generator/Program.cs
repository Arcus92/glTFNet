using glTFNet.Generator;
using glTFNet.Generator.Analyser;

var inputDirectory = Path.Combine("../../../../glTF/");
var outputDirectory = Path.Combine("../../../../");


const string jsonSerializerContextClassName = "GlTFSerializerContext";

// Defines JSON converters added to the serializer contexts
const string converterNamespace = "glTFNet.Converters";
string[] jsonConverterNames = ["Vector2Converter", "Vector3Converter", "Vector4Converter", "QuaternionConverter", "Matrix4x4Converter"];

// Load all schema files
var analyser = new SchemaAnalyser();

await AnalyseSchemaDirectory(Path.Combine(inputDirectory, "specification/2.0/schema"), "glTFNet.Models");
await AnalyseExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Archived"), "glTFNet.Extensions.Archived.Models");
await AnalyseExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Khronos"), "glTFNet.Extensions.Khronos.Models");
await AnalyseExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Vendor"), "glTFNet.Extensions.Vendor.Models");
analyser.Analyse();

// Generate the code
await GenerateModelsAndSerializerContext("glTFNet");
await GenerateModelsAndSerializerContext("glTFNet.Extensions.Archived");
await GenerateModelsAndSerializerContext("glTFNet.Extensions.Khronos");
await GenerateModelsAndSerializerContext("glTFNet.Extensions.Vendor");

return;

async Task GenerateModelsAndSerializerContext(string rootNamespace)
{
    var rootPath = Path.Combine(outputDirectory, rootNamespace);
    var modelNamespace = $"{rootNamespace}.Models";
    
    // Only export types found in this namespace
    var types = analyser.Types.Where(t => t.Namespace.StartsWith(modelNamespace)).ToList();
    
    var codeGenerator = new SchemaCodeGenerator(rootPath, rootNamespace, converterNamespace, jsonConverterNames);
    
    // Generate the model classes and enums
    await codeGenerator.WriteModelTypes(types);

    // Generate the serializer context class
    await codeGenerator.WriteJsonSerializerContext(types, jsonSerializerContextClassName, rootNamespace);
}

// Analyzes a schema directory with the given namespace
async Task AnalyseSchemaDirectory(string directory, string ns)
{
    await analyser.AddDirectory(directory, ns);
}

// Analyzes a directory with extension schemas
async Task AnalyseExtensionDirectory(string directory, string ns)
{
    foreach (var extensionDirectory in Directory.EnumerateDirectories(directory))
    {
        var extensionDirectoryName = Path.GetFileName(extensionDirectory);
        var extensionName = SchemaAnalyser.GetCSharpNamespace(extensionDirectoryName);
        var extensionNamespace = $"{ns}.{extensionName}";
        
        var extensionSchemaDirectory = Path.Combine(extensionDirectory, "schema");
        if (!Directory.Exists(extensionSchemaDirectory))
        {
            continue;
        }

        await AnalyseSchemaDirectory(extensionSchemaDirectory, extensionNamespace);
    }
}