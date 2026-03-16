using System.Collections;
using glTFNet.IO.Interfaces;

namespace glTFNet.Tests;

[TestClass]
public class GltfIndexedListRefTests
{
    private static readonly string[] Values = ["item #0", "item #1", "item #2", "item #3"];
    private static readonly int[] Indices = [1, 3];
    private static readonly IGltfContext Context = null!;
    
    [TestMethod]
    public void GltfIndexedListRef_Count()
    {
        var list = new GltfIndexedListRef<string>(Context, Values, Indices);
        
        Assert.AreEqual(2, list.Count);
    }
    
    [TestMethod]
    public void GltfIndexedListRef_Indices()
    {
        var list = new GltfIndexedListRef<string>(Context, Values, Indices);
        
        Assert.AreEqual(1, list[0].Index);
        Assert.AreEqual(3, list[1].Index);
    }
    
    [TestMethod]
    public void GltfIndexedListRef_Index()
    {
        var list = new GltfIndexedListRef<string>(Context, Values, Indices);
        
        Assert.AreEqual("item #1", list[0].Data);
        Assert.AreEqual("item #3", list[1].Data);
    }
    
    [TestMethod]
    public void GltfIndexedListRef_Enumerator()
    {
        var list = new GltfIndexedListRef<string>(Context, Values, Indices);

        using var enumerator = list.GetEnumerator();
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #1", enumerator.Current.Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #3", enumerator.Current.Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
    
    [TestMethod]
    public void GltfIndexedListRef_Enumerator_Interface()
    {
        var list = new GltfIndexedListRef<string>(Context, Values, Indices);

        var enumerator = ((IEnumerable)list).GetEnumerator();
        using var enumerator1 = enumerator as IDisposable;
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #1", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsTrue(enumerator.MoveNext());
        Assert.AreEqual("item #3", ((GltfRef<string>)enumerator.Current).Data);
        Assert.IsFalse(enumerator.MoveNext());
    }
}