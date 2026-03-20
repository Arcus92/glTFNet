using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;

namespace glTFNet.Tests.Helper;

internal static class TestExtensions
{
    /// <summary>
    /// Creates a mocked <see cref="GltfRef{T}"/> from the given glTF model.
    /// </summary>
    /// <param name="model">The glTF model.</param>
    /// <param name="resourceResolver">An optional resource resolver.</param>
    /// <returns>Returns a mocked reference.</returns>
    public static GltfRef<Gltf> MockRef(this Gltf model, IResourceResolver? resourceResolver = null)
    {
        var context = new MockContext(model)
        {
            ResourceResolver = resourceResolver
        };
        return new GltfRef<Gltf>(context, model);
    }
}