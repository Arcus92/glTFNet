using System.Collections;
using glTFNet.IO.Interfaces;

namespace glTFNet.Tests;

[TestClass]
public class GltfListRefTests
{
    private static readonly string[] Values = ["item #0", "item #1", "item #2", "item #3"];
    private static readonly IGltfContext Context = null!;
    
    [TestMethod]
    public void GltfListRef_Count()
    {
        var list = new GltfListRef<string>(Context, Values);
        
        Assert.AreEqual(4, list.Count);
    }
    
    [TestMethod]
    public void GltfListRef_Indices()
    {
        var list = new GltfListRef<string>(Context, Values);
        
        Assert.AreEqual(0, list[0].Index);
        Assert.AreEqual(1, list[1].Index);
    }
    
    [TestMethod]
    public void GltfListRef_Index()
    {
        var list = new GltfListRef<string>(Context, Values);
        
        Assert.AreEqual("item #0", list[0].Data);
        Assert.AreEqual("item #1", list[1].Data);
    }
    
    [TestMethod]
    public void GltfListRef_Enumerator()
    {
        var list = new GltfListRef<string>(Context, Values);

        using var enumerator = list.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #0", enumerator.Current.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #1", enumerator.Current.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #2", enumerator.Current.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #3", enumerator.Current.Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
    
    [TestMethod]
    public void GltfListRef_Enumerator_Interface()
    {
        var list = new GltfListRef<string>(Context, Values);

        var enumerator = ((IEnumerable)list).GetEnumerator();
        using var enumerator1 = enumerator as IDisposable;
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #0", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #1", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #2", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #3", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
}