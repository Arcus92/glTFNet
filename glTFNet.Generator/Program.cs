using glTFNet.Generator;

var schemaDirectory = Path.Combine("../../../../glTF/specification/2.0/schema");
var outputDirectory = Path.Combine("../../../../glTFNet/Models");
var outputNamespace = "glTFNet.Models";

// Load all schema files
var schemaLoader = new JsonSchemaLoader();
await schemaLoader.LoadSchemasFromDirectory(schemaDirectory);

var schemaAnalyser = new SchemaAnalyser(schemaLoader, outputNamespace);
foreach (var schema in schemaLoader)
{
    schemaAnalyser.Analyse(schema);
}

var codeGenerator = new SchemaCodeGenerator();
foreach (var type in schemaAnalyser.Types)
{
    await codeGenerator.Export(type, outputDirectory);
}