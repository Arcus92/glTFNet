using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;
using glTFNet.Tests.Helper;
using Buffer = glTFNet.Specifications.Models.Buffer;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefAccessorTests
{
    // Example glTF model
    private static readonly Gltf Model = new()
    {
        Asset = new Asset { Version = "2.0" },
        Accessors = 
        [
            new Accessor { Name = "accessor #0", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.UnsignedByte, Count = 1, BufferView = 0 },
            new Accessor { Name = "accessor #1", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.UnsignedByte, Count = 0 },
            // Buffer views
            new Accessor { Name = "accessor #2", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.UnsignedByte, Count = 1, BufferView = 0 },
            new Accessor { Name = "accessor #3", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Byte, Count = 1, BufferView = 1 },
            new Accessor { Name = "accessor #4", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.UnsignedShort, Count = 1, BufferView = 2 },
            new Accessor { Name = "accessor #5", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Short, Count = 1, BufferView = 3 },
            new Accessor { Name = "accessor #6", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.UnsignedInt, Count = 1, BufferView = 4 },
            new Accessor { Name = "accessor #7", Type = AccessorType.Scalar, ComponentType = AccessorComponentType.Float, Count = 1, BufferView = 5 }
        ],
        BufferViews = 
        [
            new BufferView { Name = "bufferView #0", Buffer = 0, ByteLength = 1, ByteOffset = 0 },
            new BufferView { Name = "bufferView #1", Buffer = 0, ByteLength = 1, ByteOffset = 1 },
            new BufferView { Name = "bufferView #2", Buffer = 0, ByteLength = 2, ByteOffset = 2 },
            new BufferView { Name = "bufferView #3", Buffer = 0, ByteLength = 2, ByteOffset = 4 },
            new BufferView { Name = "bufferView #3", Buffer = 0, ByteLength = 4, ByteOffset = 6 },
            new BufferView { Name = "bufferView #3", Buffer = 0, ByteLength = 4, ByteOffset = 10 }
        ],
        Buffers = [new Buffer { Name = "buffer #0", ByteLength = 2 }]
    };

    private static readonly IResourceResolver ResourceResolver = new MockResourceResolver()
        .Add([
            // UnsignedByte
            0x01,
            // Byte
            0x01,
            // UnsignedShort
            0x01, 0x00,
            // Short
            0x01, 0x00,
            // UnsignedInt
            0x01, 0x00, 0x00, 0x00,
            // Float
            0x00, 0x00, 0x80, 0x3f
        ]);

    [TestMethod]
    public void GltfRef_Accessor_BufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[0];
        var bufferView = item.BufferView();
        Assert.IsTrue(bufferView.HasValue);
        Assert.AreEqual(0, bufferView!.Value.Index);
        Assert.AreEqual("bufferView #0", bufferView.Value.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_BufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[1];
        var bufferView = item.BufferView();
        Assert.IsFalse(bufferView.HasValue);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_HasBufferView()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[0];
        Assert.IsTrue(item.HasBufferView(out var bufferView));
        Assert.AreEqual(0, bufferView.Index);
        Assert.AreEqual("bufferView #0", bufferView.Data.Name);
    }
    
    [TestMethod]
    public void GltfRef_Accessor_HasBufferView_Empty()
    {
        var gltf = Model.MockRef();

        var item = gltf.Accessors()[1];
        Assert.IsFalse(item.HasBufferView(out var bufferView));
        Assert.IsNull(bufferView.Data);
    }

    [TestMethod]
    public async Task GltfRef_Accessor_Read()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[0];
        var data = await item.Read();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.IsInstanceOfType<byte[]>(data);
        var dataArray = (byte[])data;
        Assert.AreEqual(1, dataArray[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_Empty()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[1];
        var data = await item.Read();
        Assert.IsNull(data);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_UnsignedByte()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[2];
        var data = await item.Read<byte>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1, data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_Byte()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[3];
        var data = await item.Read<sbyte>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1, data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_UnsingedShort()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[4];
        var data = await item.Read<ushort>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1, data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_Short()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[5];
        var data = await item.Read<short>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1, data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_UnsingedInt()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[6];
        var data = await item.Read<uint>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1, (int)data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_Float()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[7];
        var data = await item.Read<float>();
        Assert.IsNotNull(data);
        Assert.HasCount(1, data);
        Assert.AreEqual(1f, data[0]);
    }
    
    [TestMethod]
    public async Task GltfRef_Accessor_Read_Type_Empty()
    {
        var gltf = Model.MockRef(ResourceResolver);
        var item = gltf.Accessors()[1];
        var data = await item.Read<byte>();
        Assert.IsNull(data);
    }
}