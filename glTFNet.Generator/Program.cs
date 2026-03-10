using glTFNet.Generator;
using glTFNet.Generator.Analyser;

var inputDirectory = Path.Combine("../../../../glTF/");
var outputDirectory = Path.Combine("../../../../");
var outputNamespace = "glTFNet.Models";

// Load all schema files
var analyser = new SchemaAnalyser();

await AddSchemaDirectory(Path.Combine(inputDirectory, "specification/2.0/schema"), outputNamespace);
await AddExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Archived"), $"{outputNamespace}.Extensions.Archived");
await AddExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Khronos"), $"{outputNamespace}.Extensions.Khronos");
await AddExtensionDirectory(Path.Combine(inputDirectory, "extensions/2.0/Vendor"), $"{outputNamespace}.Extensions.Vendor");

analyser.Analyse();

// Generate the files
var codeGenerator = new SchemaCodeGenerator();
foreach (var type in analyser.Types)
{
    await codeGenerator.Export(type, outputDirectory);
}

return;

// Analyzes a schema directory with the given namespace
async Task AddSchemaDirectory(string directory, string ns)
{
    await analyser.AddDirectory(directory, ns);
}

// Analyzes a directory with extension schemas
async Task AddExtensionDirectory(string directory, string ns)
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

        await AddSchemaDirectory(extensionSchemaDirectory, extensionNamespace);
    }
}