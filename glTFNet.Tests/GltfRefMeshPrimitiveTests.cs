using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefMeshPrimitiveTests
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
                        {
                            { "POSITION", 0 },
                        },
                        Indices = 0,
                        Material = 0
                    },
                    new MeshPrimitive
                    {
                        Attributes = new Dictionary<string, int>(),
                    }
                ]
            },
        ],
        Materials = [new Material { Name = "material #0" }],
        Accessors = [new Accessor { Name = "accessor #0", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Short, Count = 0 }]
    };
    
    // Example glTF model with missing accessor array
    private static readonly Gltf Empty = new()
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
                        {
                            { "POSITION", 0 },
                        },
                        Indices = 0
                    }
                ]
            },
        ],
    };

    [TestMethod]
    public void GltfRef_MeshPrimitive_Indices()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0].Primitives()[0];
        var indices = item.Indices();
        Assert.IsTrue(indices.HasValue);
        Assert.AreEqual(0, indices!.Value.Index);
        Assert.AreEqual("accessor #0", indices.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_MeshPrimitive_Indices_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0].Primitives()[1];
        var indices = item.Indices();
        Assert.IsFalse(indices.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_MeshPrimitive_Material()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0].Primitives()[0];
        var material = item.Material();
        Assert.IsTrue(material.HasValue);
        Assert.AreEqual(0, material!.Value.Index);
        Assert.AreEqual("material #0", material.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_MeshPrimitive_Material_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0].Primitives()[1];
        var material = item.Material();
        Assert.IsFalse(material.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_MeshPrimitive_Attributes()
    {
        var gltf = Model.MockRef();

        var item = gltf.Meshes()[0].Primitives()[0];
        var attributes = item.Attributes();
        Assert.AreEqual(1, attributes.Count);
        Assert.IsTrue(attributes.ContainsKey("POSITION"));
    }
    
    [TestMethod]
    public void GltfRef_MeshPrimitive_Attributes_Invalid()
    {
        var gltf = Empty.MockRef();

        var item = gltf.Meshes()[0].Primitives()[0];
        var attributes = item.Attributes();
        Assert.AreEqual(0, attributes.Count);
    }
}