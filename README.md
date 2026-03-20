# glTFNet

A fast and modern glTF reader.

This library can read binary and JSON-based glTF 2.0 files.

## Generator

This project contains a C# code generator to convert the schema definition provided by Khronos to model classes.

## Usage

This library provides a wrapper struct `GltfRef<T>` with extension methods for easy access to the glFT models.

```csharp
var inputFile = "Examples/glTF-Binary/Avocado.glb";

// Loading the glTF data
await using var file = new GltfFile();
var gltf = await file.Open(inputFile);

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

External resources in glTF _(like textures or mesh data)_ are linked via an _URI_. A `FileResolver` for the parent 
directory is automatically added as `IResourceResolver`, if the `GltfFile` is loaded by from a filename. 
Custom resolvers can implement the `IResourceResolver` interface and overwrite the `GltfFile.ResourceResolver` property:

```csharp
await using var file = new GltfFile();
file.ResourceResolver = new MyCustomResourceResolver();
```

### Extensions

The glTF extension can be used by importing the additional extension packages: `glTFNet.Extensions.Khronos`, 
`glTFNet.Extensions.Vendor` or `glTFNet.Extensions.Archived`.

Make sure to register the extension in the `GltfFile` before using them:

```csharp
await using var file = new GltfFile();

// Register the extension set
file.UseKhronosExtensions();

var gltf = await file.Open(inputFile);
```

Extension data cam be read with `TryGetExtension`:

```csharp
var material = gltf.Materials()[0];

// Accessing the extension data
if (material.TryGetExtension("KHR_materials_ior", out MaterialKHRMaterialsIor extension))
{
    var ior = extension.Ior;
    
    // ...
}
```

When adding custom extension, make sure to create a JSON serializer context and add to the `GltfFile`:

```csharp
// For example: Create an extension method for your extension
public static void UseMyCustomExtension(this GltfSerializer serializer)
{
    serializer.AddSerializerContext(MyCustomExtensionSerializerContext.Default);
}
```

```csharp
// Register your extension when reading the file
await using var file = new GltfFile();
file.UseMyCustomExtension();
```