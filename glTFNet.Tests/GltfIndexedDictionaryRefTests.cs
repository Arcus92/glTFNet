using System.Collections;
using glTFNet.IO.Interfaces;

namespace glTFNet.Tests;

[TestClass]
public class GltfIndexedDictionaryRefTests
{
    private static readonly string[] Values = ["item #0", "item #1", "item #2", "item #3"];
    private static readonly Dictionary<string, int> Indices = new()
    {
        { "#1", 1 },
        { "#3", 3 }
    };
    private static readonly IGltfContext Context = null!;
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Count()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        Assert.AreEqual(2, list.Count);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Indices()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        Assert.AreEqual(1, list["#1"].Index);
        Assert.AreEqual(3, list["#3"].Index);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Index()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        Assert.AreEqual("item #1", list["#1"].Data);
        Assert.AreEqual("item #3", list["#3"].Data);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Enumerator()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);

        using var enumerator = list.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("#1", enumerator.Current.Key);
        Assert.AreEqual("item #1", enumerator.Current.Value.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("#3", enumerator.Current.Key);
        Assert.AreEqual("item #3", enumerator.Current.Value.Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Enumerator_Interface()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);

        var enumerator = ((IEnumerable)list).GetEnumerator();
        using var enumerator1 = enumerator as IDisposable;
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("#1", ((KeyValuePair<string, GltfRef<string>>)enumerator.Current).Key);
        Assert.AreEqual("item #1", ((KeyValuePair<string, GltfRef<string>>)enumerator.Current).Value.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("#3", ((KeyValuePair<string, GltfRef<string>>)enumerator.Current).Key);
        Assert.AreEqual("item #3", ((KeyValuePair<string, GltfRef<string>>)enumerator.Current).Value.Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_TryGetValue_True()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        Assert.IsTrue(list.TryGetValue("#1", out var value));
        Assert.AreEqual("item #1", value.Data);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_TryGetValue_False()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        Assert.IsFalse(list.TryGetValue("#0", out var value));
        Assert.IsNull(value.Data);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Keys()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        var keys = list.Keys.ToArray();
        Assert.HasCount(2, keys);
        Assert.AreEqual("#1", keys[0]);
        Assert.AreEqual("#3", keys[1]);
    }
    
    [TestMethod]
    public void GltfIndexedDictionaryRef_Values()
    {
        var list = new GltfIndexedDictionaryRef<string, string>(Context, Values, Indices);
        
        var values = list.Values.ToArray();
        Assert.HasCount(2, values);
        Assert.AreEqual("item #1", values[0].Data);
        Assert.AreEqual("item #3", values[1].Data);
    }
}