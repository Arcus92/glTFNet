using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefNodeTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Nodes =
        [
            new Node { Name = "node #0", Children = [1], Camera = 0, Mesh = 0 },
            new Node { Name = "node #1" }
        ],
        Cameras = [new Camera { Name = "camera #0", Type = CameraType.Perspective }],
        Meshes = [new Mesh{ Name = "mesh #0", Primitives = [] }]
    };

    [TestMethod]
    public void GltfRef_Node_Children()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[0];
        var children = item.Children();
        Assert.AreEqual(1, children.Count);
        Assert.AreEqual(1, children[0].Index);
        Assert.AreEqual("node #1", children[0].Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Node_Children_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[1];
        var children = item.Children();
        Assert.AreEqual(0, children.Count);
    }
    
    [TestMethod]
    public void GltfRef_Node_Camera()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[0];
        var camera = item.Camera();
        Assert.IsTrue(camera.HasValue);
        Assert.AreEqual(0, camera!.Value.Index);
        Assert.AreEqual("camera #0", camera.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Node_Camera_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[1];
        var camera = item.Camera();
        Assert.IsFalse(camera.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Node_Mesh()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[0];
        var mesh = item.Mesh();
        Assert.IsTrue(mesh.HasValue);
        Assert.AreEqual(0, mesh!.Value.Index);
        Assert.AreEqual("mesh #0", mesh.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Node_Mesh_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Nodes()[1];
        var mesh = item.Mesh();
        Assert.IsFalse(mesh.HasValue);
    }
}