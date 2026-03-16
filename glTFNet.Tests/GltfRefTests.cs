using glTFNet.Specifications.Models;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Scene = 0,
        Scenes = [new Scene { Name = "scene #0" }],
        Nodes = [new Node { Name = "node #0" }],
        Cameras = [new Camera { Name = "camera #0", Type = CameraType.Perspective }],
        Materials = [new Material { Name = "material #0" }],
        Textures = [new Texture { Name = "texture #0" }],
        Samplers = [new Sampler { Name = "sampler #0" }],
        Images = [new Image { Name = "image #0" }],
        Meshes = [new Mesh { Name = "mesh #0", Primitives = [] }],
        Buffers = [new Buffer { Name = "buffer #0", ByteLength = 0 }],
        BufferViews = [new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 0 }],
        Accessors =
        [
            new Accessor
            {
                Type = AccessorType.Vec3, ComponentType = AccessorComponentType.Float, Count = 0, Name = "accessor #0"
            }
        ],
        Animations = [new Animation { Name = "animation #0", Channels = [], Samplers = [] }],
        Skins = [new Skin { Name = "skin #0", Joints = [] }],
    };

    // Empty glTF model
    private static readonly Gltf Empty = new()
    {
        Asset = new Asset { Version = "2.0" },
    };

    [TestMethod]
    public void GltfRef_TryGetValue_True()
    {
        var model = Model.MockRef();

        var nullableScene = model.Scene();
        var result = nullableScene.TryGetValue(out var scene);
        Assert.IsTrue(result);
        Assert.AreEqual("scene #0", scene.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_TryGetValue_False()
    {
        var model = Empty.MockRef();

        var nullableScene = model.Scene();
        var result = nullableScene.TryGetValue(out var scene);
        Assert.IsFalse(result);
        Assert.IsNull(scene.Data);
    }
    
    [TestMethod]
    public void GltfRef_ImplicitCast()
    {
        var model = Model.MockRef();

        var scene = model.Scene()!.Value;
        Scene item = scene;
        Assert.IsNotNull(item);
        Assert.AreEqual("scene #0", item.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Asset()
    {
        var gltf = Model.MockRef();

        var item = gltf.Asset();
        Assert.IsTrue(item.HasValue);
        Assert.AreEqual(-1, item!.Value.Index);
        Assert.AreEqual("2.0", item.Value.Data.Version);
    }

    [TestMethod]
    public void GltfRef_Gltf_Scene()
    {
        var gltf = Model.MockRef();

        var item = gltf.Scene();
        Assert.IsTrue(item.HasValue);
        Assert.AreEqual(0, item!.Value.Index);
        Assert.AreEqual("scene #0", item.Value.Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Scene_Empty()
    {
        var gltf = Empty.MockRef();

        var item = gltf.Scene();
        Assert.IsFalse(item.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Gltf_HasScene()
    {
        var gltf = Model.MockRef();
        
        Assert.IsTrue(gltf.HasScene(out var item));
        Assert.AreEqual(0, item.Index);
        Assert.AreEqual("scene #0", item.Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_HasScene_Empty()
    {
        var gltf = Empty.MockRef();
        
        Assert.IsFalse(gltf.HasScene(out var item));
        Assert.IsNull(item.Data);
    }

    [TestMethod]
    public void GltfRef_Gltf_Scenes()
    {
        var gltf = Model.MockRef();

        var items = gltf.Scenes();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("scene #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Scenes_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Scenes();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Nodes()
    {
        var gltf = Model.MockRef();

        var items = gltf.Nodes();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("node #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Nodes_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Nodes();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Cameras()
    {
        var gltf = Model.MockRef();

        var items = gltf.Cameras();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("camera #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Cameras_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Cameras();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Materials()
    {
        var gltf = Model.MockRef();

        var items = gltf.Materials();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("material #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Materials_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Materials();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Textures()
    {
        var gltf = Model.MockRef();

        var items = gltf.Textures();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("texture #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Textures_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Textures();
        Assert.AreEqual(0, items.Count);
    }
    
    [TestMethod]
    public void GltfRef_Gltf_Images()
    {
        var gltf = Model.MockRef();

        var items = gltf.Images();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("image #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Images_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Images();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Samplers()
    {
        var gltf = Model.MockRef();

        var items = gltf.Samplers();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("sampler #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Samplers_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Samplers();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Meshes()
    {
        var gltf = Model.MockRef();

        var items = gltf.Meshes();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("mesh #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Meshes_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Meshes();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Buffers()
    {
        var gltf = Model.MockRef();

        var items = gltf.Buffers();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("buffer #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Buffers_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Buffers();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_BufferViews()
    {
        var gltf = Model.MockRef();

        var items = gltf.BufferViews();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("bufferView #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_BufferViews_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.BufferViews();
        Assert.AreEqual(0, items.Count);
    }

    [TestMethod]
    public void GltfRef_Gltf_Accessors()
    {
        var gltf = Model.MockRef();

        var items = gltf.Accessors();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("accessor #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Accessors_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Accessors();
        Assert.AreEqual(0, items.Count);
    }
    
    [TestMethod]
    public void GltfRef_Gltf_Animations()
    {
        var gltf = Model.MockRef();

        var items = gltf.Animations();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("animation #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Animations_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Animations();
        Assert.AreEqual(0, items.Count);
    }
    
    [TestMethod]
    public void GltfRef_Gltf_Skins()
    {
        var gltf = Model.MockRef();

        var items = gltf.Skins();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(0, items[0].Index);
        Assert.AreEqual("skin #0", items[0].Data.Name);
    }

    [TestMethod]
    public void GltfRef_Gltf_Skin_Empty()
    {
        var gltf = Empty.MockRef();

        var items = gltf.Skins();
        Assert.AreEqual(0, items.Count);
    }
}