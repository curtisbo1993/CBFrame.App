using System.Collections.Generic;

namespace CBFrame.SectionsDb.Models
{
    /// <summary>
    /// Root object for the sections JSON database.
    /// </summary>
    public sealed class SectionDatabase
    {
        /// <summary>
        /// Optional description (e.g. "AISC (example subset)").
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Units system identifier (e.g. "US", "SI").
        /// </summary>
        public string Units { get; set; } = "US";

        /// <summary>
        /// All sections in this database.
        /// </summary>
        public List<SectionShape> Sections { get; set; } = new();
    }
}
