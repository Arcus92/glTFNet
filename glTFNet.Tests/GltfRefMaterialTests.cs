using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefMaterialTests
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
                PbrMetallicRoughness = new MaterialPbrMetallicRoughness(),
                NormalTexture = new MaterialNormalTextureInfo { Index = 0 },
                OcclusionTexture = new MaterialOcclusionTextureInfo { Index = 0 },
                EmissiveTexture = new TextureInfo { Index = 0 }
            },
            new Material { Name = "material #1" }
        ],
        Textures = [ new Texture { Name = "texture #0" } ]
    };

    [TestMethod]
    public void GltfRef_Material_PbrMetallicRoughness()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0];
        var pbrMetallicRoughness = item.PbrMetallicRoughness();
        Assert.IsTrue(pbrMetallicRoughness.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_PbrMetallicRoughness_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1];
        var pbrMetallicRoughness = item.PbrMetallicRoughness();
        Assert.IsFalse(pbrMetallicRoughness.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_NormalTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0];
        var normalTexture = item.NormalTexture();
        Assert.IsTrue(normalTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_NormalTexture_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1];
        var normalTexture = item.NormalTexture();
        Assert.IsFalse(normalTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_OcclusionTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0];
        var occlusionTexture = item.OcclusionTexture();
        Assert.IsTrue(occlusionTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_OcclusionTexture_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1];
        var occlusionTexture = item.OcclusionTexture();
        Assert.IsFalse(occlusionTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_EmissiveTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0];
        var emissiveTexture = item.EmissiveTexture();
        Assert.IsTrue(emissiveTexture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Material_EmissiveTexture_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[1];
        var emissiveTexture = item.EmissiveTexture();
        Assert.IsFalse(emissiveTexture.HasValue);
    }
}