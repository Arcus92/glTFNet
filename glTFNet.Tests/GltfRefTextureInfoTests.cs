using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefTextureInfoTests
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
                }
            }
        ],
        Textures = 
        [
            new Texture { Name = "texture #0" },
        ]
    };
    
    // Example glTF model with missing texture array
    private static readonly Gltf Empty = new()
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
                }
            }
        ]
    };

    [TestMethod]
    public void GltfRef_TextureInfo_Texture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value.BaseColorTexture()!.Value;
        var texture = item.Texture();
        Assert.IsTrue(texture.HasValue);
        Assert.AreEqual(0, texture!.Value.Index);
        Assert.AreEqual("texture #0", texture.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_TextureInfo_Texture_Empty()
    {
        var gltf = Empty.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value.BaseColorTexture()!.Value;
        var texture = item.Texture();
        Assert.IsFalse(texture.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_TextureInfo_HasTexture()
    {
        var gltf = Model.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value.BaseColorTexture()!.Value;
        Assert.IsTrue(item.HasTexture(out var texture));
        Assert.AreEqual(0, texture.Index);
        Assert.AreEqual("texture #0", texture.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_TextureInfo_HasTexture_Empty()
    {
        var gltf = Empty.MockRef();

        var item = gltf.Materials()[0].PbrMetallicRoughness()!.Value.BaseColorTexture()!.Value;
        Assert.IsFalse(item.HasTexture(out var texture));
        Assert.IsNull(texture.Data);
    }
}