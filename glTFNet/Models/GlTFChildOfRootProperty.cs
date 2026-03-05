namespace glTFNet.Models
{
    [System.Serializable]
    public class GlTFChildOfRootProperty : GlTFProperty
    {
        /// <summary>
        /// The user-defined name of this object.  This is not necessarily unique, e.g., an accessor and a buffer could have the same name, or two accessors could even have the same name.
        /// </summary>
        public System.String? Name { get; set; }
    }
}