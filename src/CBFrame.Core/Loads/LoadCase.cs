using CBFrame.Core.Enums;

namespace CBFrame.Core.Loads
{
    /// <summary>
    /// A single load case (e.g. Dead, Live, Wind X).
    /// Data only in Phase 2.
    /// </summary>
    public class LoadCase
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Untitled Load Case";

        public LoadCaseType CaseType { get; set; } = LoadCaseType.Other;

        /// <summary>
        /// Optional description or notes.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if this case is primary (vs. generated, such as wind envelope).
        /// </summary>
        public bool IsPrimary { get; set; } = true;
    }
}
