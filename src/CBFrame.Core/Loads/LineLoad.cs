using CBFrame.Core.Elements;
using CBFrame.Core.Enums;

namespace CBFrame.Core.Loads
{
    /// <summary>
    /// Distributed load applied along a line element.
    /// No shape functions in Phase 2 – just data.
    /// </summary>
    public class LineLoad
    {
        public int Id { get; set; }

        public LineElement Element { get; set; } = null!;

        public int LoadCaseId { get; set; }

        /// <summary>
        /// Load direction (global or local).
        /// </summary>
        public LoadDirection Direction { get; set; } = LoadDirection.GlobalZ;

        /// <summary>
        /// Distribution type (uniform, trapezoidal, etc.).
        /// </summary>
        public LoadDistributionType DistributionType { get; set; } = LoadDistributionType.Uniform;

        /// <summary>
        /// Start magnitude at I-end of element.
        /// </summary>
        public double StartMagnitude { get; set; }

        /// <summary>
        /// End magnitude at J-end of element.
        /// </summary>
        public double EndMagnitude { get; set; }
    }
}
