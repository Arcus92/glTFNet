# glTFNet

A fast and mordern glTF reader.

This library can read binary and JSON-based glTF 2.0 files.

## Generator

This project contains a C# code generator to convert the schema definition provided by Khronos to model classes.

## Example

```csharp
var inputFile = "Examples/glTF-Binary/Avocado.glb";

// Loading the glTF data
await using var loader = new GlTFLoader();
var gltf = await loader.Open(inputFile);

var scene = gltf.Scene;
if (!scene.HasValue) return;

foreach (var node in scene.Value.Nodes)
{
    if (!node.Mesh.HasValue) continue;
    
    foreach (var meshPrimitive in node.Mesh.Value.Primitives)
    {
        if (!meshPrimitive.Indices.HasValue) continue;
        
        // Lazy-loading the indices from the gbl file
        var indices = await meshPrimitive.Indices.Value.Read();
        
        // ...
    }
}
```