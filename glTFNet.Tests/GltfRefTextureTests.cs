using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefTextureTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Textures = 
        [
            new Texture { Name = "texture #0", Source = 0, Sampler = 0 },
            new Texture { Name = "texture #1" } 
        ],
        Images = [ new Image { Name = "image #0" } ],
        Samplers = [ new Sampler { Name = "sampler #0" } ]
    };

    [TestMethod]
    public void GltfRef_Texture_Source()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[0];
        var source = item.Source();
        Assert.IsTrue(source.HasValue);
        Assert.AreEqual(0, source!.Value.Index);
        Assert.AreEqual("image #0", source.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Texture_Source_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[1];
        var source = item.Source();
        Assert.IsFalse(source.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Texture_HasSource()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[0];
        Assert.IsTrue(item.HasSource(out var source));
        Assert.AreEqual(0, source.Index);
        Assert.AreEqual("image #0", source.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Texture_HasSource_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[1];
        Assert.IsFalse(item.HasSource(out var source));
        Assert.IsNull(source.Data);
    }
    
    [TestMethod]
    public void GltfRef_Texture_Sampler()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[0];
        var sampler = item.Sampler();
        Assert.IsTrue(sampler.HasValue);
        Assert.AreEqual(0, sampler!.Value.Index);
        Assert.AreEqual("sampler #0", sampler.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Texture_Sampler_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[1];
        var sampler = item.Sampler();
        Assert.IsFalse(sampler.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Texture_HasSampler()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[0];
        Assert.IsTrue(item.HasSampler(out var sampler));
        Assert.AreEqual(0, sampler.Index);
        Assert.AreEqual("sampler #0", sampler.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Texture_HasSampler_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Textures()[1];
        Assert.IsFalse(item.HasSampler(out var sampler));
        Assert.IsNull(sampler.Data);
    }
}