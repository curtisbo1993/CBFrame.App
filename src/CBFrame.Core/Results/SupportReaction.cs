using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Results
{
    /// <summary>
    /// Support reactions at a node
    /// for a specific load case or combination.
    /// </summary>
    public class SupportReaction
    {
        public int Id { get; set; }

        public Node Node { get; set; } = null!;

        public int? LoadCaseId { get; set; }

        public int? LoadCombinationId { get; set; }

        public ResultSourceType SourceType { get; set; } = ResultSourceType.LoadCase;

        // Reaction forces (global)
        public double RxForce { get; set; }
        public double RyForce { get; set; }
        public double RzForce { get; set; }

        // Reaction moments (global)
        public double MxMoment { get; set; }
        public double MyMoment { get; set; }
        public double MzMoment { get; set; }
    }
}
