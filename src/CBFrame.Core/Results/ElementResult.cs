using CBFrame.Core.Elements;
using CBFrame.Core.Enums;

namespace CBFrame.Core.Results
{
    /// <summary>
    /// Force and moment results for a line element
    /// at each end for a specific load case or combination.
    /// </summary>
    public class ElementResult
    {
        public int Id { get; set; }

        public LineElement Element { get; set; } = null!;

        public int? LoadCaseId { get; set; }

        public int? LoadCombinationId { get; set; }

        public ResultSourceType SourceType { get; set; } = ResultSourceType.LoadCase;

        // End forces at I-end (local axes)
        public double FxI { get; set; }
        public double FyI { get; set; }
        public double FzI { get; set; }
        public double MxI { get; set; }
        public double MyI { get; set; }
        public double MzI { get; set; }

        // End forces at J-end (local axes)
        public double FxJ { get; set; }
        public double FyJ { get; set; }
        public double FzJ { get; set; }
        public double MxJ { get; set; }
        public double MyJ { get; set; }
        public double MzJ { get; set; }
    }
}
