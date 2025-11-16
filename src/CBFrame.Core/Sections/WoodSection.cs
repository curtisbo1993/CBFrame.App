using CBFrame.Core.Enums;

namespace CBFrame.Core.Sections
{
    /// <summary>
    /// Wood member cross-section.
    /// </summary>
    public class WoodSection : SectionBase
    {
        public WoodSection()
        {
            SectionMaterialType = SectionMaterialType.Wood;
        }

        /// <summary>
        /// Nominal width (e.g. 1.5").
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Nominal depth (e.g. 9.25").
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Actual size label (e.g. "2x10").
        /// </summary>
        public string? NominalSizeLabel { get; set; }
    }
}
