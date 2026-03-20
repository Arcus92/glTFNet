using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefImageTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Images = 
        [
            new Image { Name = "image #0", BufferView = 0 },
            new Image { Name = "image #1" }
        ],
        BufferViews = [new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 0 }]
    };

    [TestMethod]
    public void GltfRef_Image_BufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Images()[0];
        var bufferView = item.BufferView();
        Assert.IsTrue(bufferView.HasValue);
        Assert.AreEqual(0, bufferView!.Value.Index);
        Assert.AreEqual("bufferView #0", bufferView.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Image_BufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Images()[1];
        var bufferView = item.BufferView();
        Assert.IsFalse(bufferView.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Image_HasBufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Images()[0];
        Assert.IsTrue(item.HasBufferView(out var bufferView));
        Assert.AreEqual(0, bufferView.Index);
        Assert.AreEqual("bufferView #0", bufferView.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Image_HasBufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Images()[1];
        Assert.IsFalse(item.HasBufferView(out var bufferView));
        Assert.IsNull(bufferView.Data);
    }
}