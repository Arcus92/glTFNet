using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefSceneTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Scenes = 
        [
            new Scene { Name = "node #0", Nodes = [0] },
            new Scene { Name = "node #1" } 
        ],
        Nodes = [ new Node { Name = "node #0" } ]
    };

    [TestMethod]
    public void GltfRef_Scene_Nodes()
    {
        var gltf = Model.MockRef();

        var item = gltf.Scenes()[0];
        var children = item.Nodes();
        Assert.AreEqual(1, children.Count);
        Assert.AreEqual(0, children[0].Index);
        Assert.AreEqual("node #0", children[0].Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Scene_Nodes_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Scenes()[1];
        var children = item.Nodes();
        Assert.AreEqual(0, children.Count);
    }
}