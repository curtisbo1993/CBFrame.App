using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Elements
{
    /// <summary>
    /// A 1D line element connecting two nodes.
    /// Represents beams, columns, braces, etc.
    /// Pure data only in Phase 2.
    /// </summary>
    public class LineElement : ElementBase
    {
        public LineElement()
        {
            ElementType = ElementType.Line;
        }

        /// <summary>
        /// Is this element oriented as a beam, column, brace, etc.
        /// For now descriptive only.
        /// </summary>
        public LineElementUsage Usage { get; set; } = LineElementUsage.Generic;

        /// <summary>
        /// Optional orientation angle (for local axes).
        /// No math in Phase 2 — just a stored value.
        /// </summary>
        public double OrientationAngle { get; set; } = 0.0;
    }
}
