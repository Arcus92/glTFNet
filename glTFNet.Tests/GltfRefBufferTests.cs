using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefBufferTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Buffers = [new Buffer { Name = "buffer #0", ByteLength = 0 }]
    };
    
    private static readonly IResourceResolver ResourceResolver = new MockResourceResolver().Add([]);
    
    [TestMethod]
    public async Task GltfRef_Buffer_Open()
    {
        var gltf = Model.MockRef(ResourceResolver);

        var item = gltf.Buffers()[0];
        var bufferView = await item.Open();
        Assert.IsNotNull(bufferView);
    }
    
    [TestMethod]
    public async Task GltfRef_Buffer_Open_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Buffers()[0];
        await Assert.ThrowsAsync<Exception>(async () => await item.Open());
    }
}