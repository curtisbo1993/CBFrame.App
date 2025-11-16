using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Loads
{
    /// <summary>
    /// Concentrated load applied at a node.
    /// Phase 2: only stores magnitudes.
    /// </summary>
    public class NodalLoad
    {
        public int Id { get; set; }

        public Node Node { get; set; } = null!;

        public int LoadCaseId { get; set; }

        /// <summary>
        /// Direction for this load component (e.g. GlobalZ).
        /// </summary>
        public LoadDirection Direction { get; set; } = LoadDirection.GlobalZ;

        /// <summary>
        /// Load magnitude (force or moment) in model units.
        /// Positive/negative meaning is defined later in analysis.
        /// </summary>
        public double Magnitude { get; set; }
    }
}
