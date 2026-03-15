using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefPbdMetallicRoughnessTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Materials = 
        [
            new Material
            {
                Name = "material #0",
                PbrMetallicRoughness = new MaterialPbrMetallicRoughness
                {
                    BaseColorTexture = new TextureInfo { Index = 0 },
                    MetallicRoughnessTexture = new TextureInfo { Index = 0 },
                }
            },
            new Material
            {
                Name = "material #1",
                PbrMetallicRoughness = new MaterialPbrMetallicRoughness()
            }
        ],
        Textures = [ new Texture { Name = "texture #0" } ]
    };
    
    
    [TestMethod]
    public void GltfRef_PbrMetallicRoughness_BaseColorTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value;
        var baseColorTexture = item.BaseColorTexture();
        Assert.IsTrue(baseColorTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_PbrMetallicRoughness_BaseColorTexture_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1].PbrMetallicRoughness()!.Value;
        var baseColorTexture = item.BaseColorTexture();
        Assert.IsFalse(baseColorTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_PbrMetallicRoughness_MetallicRoughnessTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value;
        var metallicRoughnessTexture = item.MetallicRoughnessTexture();
        Assert.IsTrue(metallicRoughnessTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_PbrMetallicRoughness_MetallicRoughnessTexture_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1].PbrMetallicRoughness()!.Value;
        var metallicRoughnessTexture = item.MetallicRoughnessTexture();
        Assert.IsFalse(metallicRoughnessTexture.HasValue);
    }
}