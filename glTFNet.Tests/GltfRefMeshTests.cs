using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefMeshTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Meshes =
        [
            new Mesh
            {
                Name = "mesh #0", Primitives =
                [
                    new MeshPrimitive
                    {
                        Attributes = new Dictionary<string, int>()
                    }
                ]
            },
        ]
    };

    [TestMethod]
    public void GltfRef_Mesh_Primitives()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0];
        var primitives = item.Primitives();
        Assert.AreEqual(1, primitives.Count);
        Assert.AreEqual(0, primitives[0].Index);
    }
}