using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Results
{
    /// <summary>
    /// Displacement and rotation results at a node
    /// for a specific load case or combination.
    /// </summary>
    public class NodeResult
    {
        public int Id { get; set; }

        /// <summary>
        /// Node to which these results belong.
        /// </summary>
        public Node Node { get; set; } = null!;

        /// <summary>
        /// Load case ID (if this result is for a case).
        /// </summary>
        public int? LoadCaseId { get; set; }

        /// <summary>
        /// Load combination ID (if this result is for a combo).
        /// </summary>
        public int? LoadCombinationId { get; set; }

        /// <summary>
        /// Indicates whether this result comes from a case or a combo.
        /// </summary>
        public ResultSourceType SourceType { get; set; } = ResultSourceType.LoadCase;

        // Translations
        public double Ux { get; set; }
        public double Uy { get; set; }
        public double Uz { get; set; }

        // Rotations
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }
    }
}
