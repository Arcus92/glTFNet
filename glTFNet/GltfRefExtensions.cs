using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using glTFNet.IO.Interfaces;
using glTFNet.Specifications.Models;
using JetBrains.Annotations;

namespace glTFNet;

/// <summary>
/// The extension class for <see cref="GltfRef{T}"/>.
/// </summary>
[PublicAPI]
public static class GltfRefExtensions
{
    /// <summary>
    /// Tries to unwrap the nullable glTF reference.
    /// </summary>
    /// <param name="result">Returns the unwrapped reference.</param>
    /// <param name="instance">The glTF reference.</param>
    /// <returns>Returns true, if this nullable wrapper has a value.</returns>
    public static bool TryGetValue<T>(this GltfRef<T>? instance, out GltfRef<T> result)
    {
        if (!instance.HasValue)
        {
            result = default;
            return false;
        }

        result = instance.Value;
        return true;
    }
    
    /// <summary>
    /// Tries to get an extension object by the given name.
    /// </summary>
    /// <param name="extensionName">The name of the extension.</param>
    /// <param name="extension">Returns the extension if found.</param>
    /// <param name="instance">The glTF base instance.</param>
    /// <typeparam name="TExtension">The extension type.</typeparam>
    /// <typeparam name="T">The glTF model type.</typeparam>
    /// <returns>Returns true, if the extension was found.</returns>
    public static bool TryGetExtension<T, TExtension>(this GltfRef<T> instance, string extensionName, [MaybeNullWhen(false)] out TExtension extension) 
        where T : GltfProperty 
        where TExtension : class
    {
        if (instance.Data.Extensions is null || 
            !instance.Data.Extensions.TryGetValue(extensionName, out var extensionObject))
        {
            extension = null;
            return false;
        }

        // The extension is already cased
        if (extensionObject is TExtension extensionType)
        {
            extension = extensionType;
            return true;
        }
        
        // The extension is a JSON object and needs to be deserialized
        if (extensionObject is not JsonElement extensionJsonElement)
        {
            extension = null;
            return false;
        }

        // Getting the JSON type info
        var typeInfo = instance.Context.As<IGltfSerializerContext>().GetTypeInfo<TExtension>();
        if (typeInfo is null)
        {
            extension = null;
            return false;
        }

        // Deserialize the extension
        extension = extensionJsonElement.Deserialize(typeInfo)!;
        return true;
    }

    /// <summary>
    /// Sets an extension for a glTF instance.
    /// </summary>
    /// <param name="extensionName">The extension name.</param>
    /// <param name="extension">The extension to set. Set to <c>null</c> to remove the extension.</param>
    /// <param name="instance">The glTF base instance.</param>
    /// <typeparam name="TExtension">The extension type.</typeparam>
    /// <typeparam name="T">The glTF model type.</typeparam>
    public static void SetExtension<T, TExtension>(this GltfRef<T> instance, string extensionName, TExtension? extension) 
        where T : GltfProperty
        where TExtension : class
    {
        instance.Data.Extensions ??= new Extension();
        
        if (extension is null)
        {
            instance.Data.Extensions.Remove(extensionName);
        }
        else
        {
            instance.Data.Extensions[extensionName] = extension;
        }
    }
}