using System.Text.Json;
using glTFNet.Extensions.Khronos;
using glTFNet.Extensions.Khronos.Models.KhrMaterialsIor;
using glTFNet.IO;
using glTFNet.Specifications.Models;

namespace glTFNet.Tests;

[TestClass]
public class GltfRefExtensionsTests
{
    private static readonly string Json =
        """
        {
            "extensions": 
            {
                "KHR_materials_ior": { "ior": 1.5 },
                "KHR_materials_ior_null": null
            }
        }
        """;
    
    [TestMethod]
    public void GltfRef_Extensions_TryGetExtension()
    {
        var context = new GltfLoader();
        context.UseKhronosExtensions();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        Assert.IsTrue(material.TryGetExtension("KHR_materials_ior", out MaterialKHRMaterialsIor extension));
        Assert.IsNotNull(extension);
        Assert.AreEqual(1.5f, extension.Ior);
    }
    
    [TestMethod]
    public void GltfRef_Extensions_TryGetExtension_Empty()
    {
        var context = new GltfLoader();
        context.UseKhronosExtensions();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        Assert.IsFalse(material.TryGetExtension("KHR_materials_ior_empty", out MaterialKHRMaterialsIor extension));
        Assert.IsNull(extension);
    }
    
    [TestMethod]
    public void GltfRef_Extensions_TryGetExtension_Null()
    {
        var context = new GltfLoader();
        context.UseKhronosExtensions();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        Assert.IsFalse(material.TryGetExtension("KHR_materials_ior_null", out MaterialKHRMaterialsIor extension));
        Assert.IsNull(extension);
    }
    
    [TestMethod]
    public void GltfRef_Extensions_SetExtension()
    {
        var context = new GltfLoader();
        context.UseKhronosExtensions();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        material.SetExtension("KHR_materials_ior", new MaterialKHRMaterialsIor { Ior = 1.5f });
        
        Assert.IsTrue(material.TryGetExtension("KHR_materials_ior", out MaterialKHRMaterialsIor extension));
        Assert.IsNotNull(extension);
        Assert.AreEqual(1.5f, extension.Ior);
    }
    
    [TestMethod]
    public void GltfRef_Extensions_SetExtension_Remove()
    {
        var context = new GltfLoader();
        context.UseKhronosExtensions();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        material.SetExtension("KHR_materials_ior", (MaterialKHRMaterialsIor?)null);
        
        Assert.IsFalse(material.TryGetExtension("KHR_materials_ior", out MaterialKHRMaterialsIor extension));
        Assert.IsNull(extension);
    }
    
    [TestMethod]
    public void GltfRef_Extensions_TryGetExtension_ExtensionsNotRegistered()
    {
        var context = new GltfLoader();
        
        var data = JsonSerializer.Deserialize(Json, Specifications.GltfSerializerContext.Default.Material)!;
        var material = new GltfRef<Material>(context, data);
        
        Assert.IsFalse(material.TryGetExtension("KHR_materials_ior", out MaterialKHRMaterialsIor extension));
        Assert.IsNull(extension);
    }
}