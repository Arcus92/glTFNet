using glTFNet.IO;

namespace glTFNet.Tests;

[TestClass]
public class GltfLoaderIntegrationTests
{
    [TestMethod]
    public async Task GltfLoader_LoadGltf()
    {
        await using var loader = new GltfLoader();
        var gltf = await loader.Open("Examples/glTF/Avocado.gltf");

        // Verify access to the first texture image
        var image = gltf.Images()[0];
        await using var stream = await image.Open();
        Assert.AreEqual(3158729, stream.Length);
        
        // Verify access to the first mesh data
        var mesh = gltf.Meshes()[0];
        var meshPrimitive = mesh.Primitives()[0];
        var indices = await meshPrimitive.Indices()!.Value.Read();
        Assert.IsNotNull(indices);
        Assert.HasCount(2046, indices);
        Assert.IsInstanceOfType<ushort[]>(indices);
    }
    
    [TestMethod]
    public async Task GltfLoader_LoadGlb()
    {
        await using var loader = new GltfLoader();
        var gltf = await loader.Open("Examples/glTF-Binary/Avocado.glb");

        // Verify access to the first texture image
        var image = gltf.Images()[0];
        await using var stream = await image.Open();
        Assert.AreEqual(3158729, stream.Length);
        
        // Verify access to the first mesh data
        var mesh = gltf.Meshes()[0];
        var meshPrimitive = mesh.Primitives()[0];
        var indices = await meshPrimitive.Indices()!.Value.Read();
        Assert.IsNotNull(indices);
        Assert.HasCount(2046, indices);
        Assert.IsInstanceOfType<ushort[]>(indices);
    }
}