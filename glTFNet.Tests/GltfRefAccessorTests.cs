using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefAccessorTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Accessors = 
        [
            new Accessor { Name = "accessor #0", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Short, Count = 0, BufferView = 0 },
            new Accessor { Name = "accessor #0", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Short, Count = 0 }
        ],
        BufferViews = [new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 0 }]
    };

    [TestMethod]
    public void GltfRef_Accessor_BufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[0];
        var bufferView = item.BufferView();
        Assert.IsTrue(bufferView.HasValue);
        Assert.AreEqual(0, bufferView!.Value.Index);
        Assert.AreEqual("bufferView #0", bufferView.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_BufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[1];
        var bufferView = item.BufferView();
        Assert.IsFalse(bufferView.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_HasBufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[0];
        Assert.IsTrue(item.HasBufferView(out var bufferView));
        Assert.AreEqual(0, bufferView.Index);
        Assert.AreEqual("bufferView #0", bufferView.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_HasBufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[1];
        Assert.IsFalse(item.HasBufferView(out var bufferView));
        Assert.IsNull(bufferView.Data);
    }
}