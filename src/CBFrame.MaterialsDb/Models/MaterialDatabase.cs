using System.Collections.Generic;

namespace CBFrame.MaterialsDb.Models
{
    /// <summary>
    /// Root object for the materials JSON database.
    /// </summary>
    public sealed class MaterialDatabase
    {
        public string Units { get; set; } = "US";

        // List<T> must be public and the T (MaterialRecord) must be public
        public List<MaterialRecord> Materials { get; set; } = new();
    }
}
