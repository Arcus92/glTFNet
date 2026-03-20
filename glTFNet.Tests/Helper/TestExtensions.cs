using glTFNet.IO;
using glTFNet.Specifications.Models;

namespace glTFNet.Tests.Helper;

internal static class TestExtensions
{
    /// <summary>
    /// Creates a mocked <see cref="GltfRef{T}"/> from the given glTF model.
    /// </summary>
    /// <param name="model">The glTF model.</param>
    /// <returns>Returns a mocked reference.</returns>
    public static GltfRef<Gltf> MockRef(this Gltf model)
    {
        var context = new GltfNodeContext<Gltf>(model);
        return new GltfRef<Gltf>(context, model);
    }
}