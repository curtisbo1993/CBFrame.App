using CBFrame.Core.Enums;

namespace CBFrame.Core.Sections
{
    /// <summary>
    /// Concrete section (beams, columns, slabs, walls).
    /// Phase 2: geometry only, no reinforcement logic yet.
    /// </summary>
    public class ConcreteSection : SectionBase
    {
        public ConcreteSection()
        {
            SectionMaterialType = SectionMaterialType.Concrete;
        }

        /// <summary>
        /// Overall width (b).
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Overall depth (h).
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Optional thickness (for slabs/walls).
        /// </summary>
        public double Thickness { get; set; }
    }
}
