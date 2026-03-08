namespace glTFNet.Models;

/// <summary>
/// Image data used to create a texture. Image **MAY** be referenced by an URI (or IRI) or a buffer view index.
/// </summary>
[Serializable]
public class Image : GlTFChildOfRootProperty
{
    /// <summary>
    /// The URI (or IRI) of the image.  Relative paths are relative to the current glTF asset.  Instead of referencing an external file, this field **MAY** contain a `data:`-URI. This field **MUST NOT** be defined when `bufferView` is defined.
    /// </summary>
    public string? Uri { get; set; }

    /// <summary>
    /// The image's media type. This field **MUST** be defined when `bufferView` is defined.
    /// </summary>
    public ImageMimeType? MimeType { get; set; }

    /// <summary>
    /// The index of the bufferView that contains the image. This field **MUST NOT** be defined when `uri` is defined.
    /// </summary>
    public int? BufferView { get; set; }
}