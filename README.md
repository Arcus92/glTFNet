# glTFNet

A fast and modern glTF reader.

This library can read binary and JSON-based glTF 2.0 files.

## Generator

This project contains a C# code generator to convert the schema definition provided by Khronos to model classes.

## Example

```csharp
var inputFile = "Examples/glTF-Binary/Avocado.glb";

// Loading the glTF data
await using var gltfFile = new GltfFile();
var gltf = await gltfFile.Open(inputFile);

if (!gltf.HasScene(out var scene)) return;

foreach (var node in scene.Nodes())
{
    if (!node.HasMesh(out var mesh)) continue;

    foreach (var meshPrimitive in mesh.Primitives())
    {
        if (!meshPrimitive.HasIndices(out var indicesAccessor)) continue;
        if (!meshPrimitive.HasAttributes("POSITION", out var positionAccessor)) continue;
        
        // Lazy-loading the data from the gbl file
        var indices = await indicesAccessor.Read();
        var positions = await positionAccessor.Read();
        
        // ...
    }
}
```