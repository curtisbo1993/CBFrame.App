using CBFrame.Core.Enums;

namespace CBFrame.Core.Sections
{
    /// <summary>
    /// Steel section definition (W-shapes, channels, tubes, etc.).
    /// Phase 2: data only.
    /// </summary>
    public class SteelSection : SectionBase
    {
        public SteelSection()
        {
            SectionMaterialType = SectionMaterialType.Steel;
        }

        /// <summary>
        /// Designation / catalog name (e.g. "W14x90").
        /// </summary>
        public string? Designation { get; set; }

        /// <summary>
        /// Section depth (overall height).
        /// </summary>
        public double Depth { get; set; }

        /// <summary>
        /// Flange width.
        /// </summary>
        public double FlangeWidth { get; set; }

        /// <summary>
        /// Web thickness.
        /// </summary>
        public double WebThickness { get; set; }

        /// <summary>
        /// Flange thickness.
        /// </summary>
        public double FlangeThickness { get; set; }
    }
}
