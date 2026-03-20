using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefBufferViewTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        BufferViews = 
        [
            new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 0 }
        ],
        Buffers = [new Buffer { Name = "buffer #0", ByteLength = 0 }]
    };
    
    // Example glTF model with missing buffer array
    private static readonly Gltf Empty = new()
    {
        Asset = new Asset { Version = "2.0" },
        BufferViews = 
        [
            new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 0 }
        ]
    };

    [TestMethod]
    public void GltfRef_BufferView_Buffer()
    {
        var gltf = Model.MockRef();

        var item = gltf.BufferViews()[0];
        var buffer = item.Buffer();
        Assert.IsTrue(buffer.HasValue);
        Assert.AreEqual(0, buffer!.Value.Index);
        Assert.AreEqual("buffer #0", buffer.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_BufferView_Buffer_Empty()
    {
        var gltf = Empty.MockRef();

        var item = gltf.BufferViews()[0];
        var buffer = item.Buffer();
        Assert.IsFalse(buffer.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_BufferView_HasBuffer()
    {
        var gltf = Model.MockRef();

        var item = gltf.BufferViews()[0];
        Assert.IsTrue(item.HasBuffer(out var buffer));
        Assert.AreEqual(0, buffer.Index);
        Assert.AreEqual("buffer #0", buffer.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_BufferView_HasBuffer_Empty()
    {
        var gltf = Empty.MockRef();

        var item = gltf.BufferViews()[0];
        Assert.IsFalse(item.HasBuffer(out var buffer));
        Assert.IsNull(buffer.Data);
    }
}