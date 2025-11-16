using System.Collections.Generic;

namespace CBFrame.Core.Loads
{
    /// <summary>
    /// A load combination (e.g. 1.2D + 1.6L).
    /// We only store factors here; no design logic yet.
    /// </summary>
    public class LoadCombination
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Untitled Combination";

        /// <summary>
        /// Map of LoadCaseId → factor.
        /// </summary>
        public IDictionary<int, double> Factors { get; } = new Dictionary<int, double>();

        /// <summary>
        /// Optional description or code reference (e.g. "ASCE 7-16 2.3.2").
        /// </summary>
        public string? Description { get; set; }
    }
}
