using glTFNet.Generator;

var schemaDirectory = Path.Combine("../../../../glTF/specification/2.0/schema");
var sourceDirectory = Path.Combine("../../../../glTFNet/Models");

var schemaLoader = new JsonSchemaLoader();
var codeGenerator = new SchemaCodeGenerator(schemaLoader, sourceDirectory, "glTFNet.Models");
await schemaLoader.LoadSchemasFromDirectoryAsync(schemaDirectory);

foreach (var schema in schemaLoader)
{
    if (schema.Type != "object") continue;
    await codeGenerator.ExportAsync(schema);
}