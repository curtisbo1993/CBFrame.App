using CBFrame.Core.Enums;

namespace CBFrame.Core.Materials
{
    /// <summary>
    /// Wood material definition.
    /// Phase 2: basic strengths and classification only.
    /// </summary>
    public class WoodMaterial : MaterialBase
    {
        public WoodMaterial()
        {
            MaterialType = MaterialType.Wood;
        }

        /// <summary>
        /// Species or species group (e.g. "SPF", "DF-L").
        /// </summary>
        public string? Species { get; set; }

        /// <summary>
        /// Bending design value Fb.
        /// </summary>
        public double BendingStrength { get; set; }

        /// <summary>
        /// Axial compression parallel to grain Fc.
        /// </summary>
        public double CompressionParallel { get; set; }

        /// <summary>
        /// Tension parallel to grain Ft.
        /// </summary>
        public double TensionParallel { get; set; }

        /// <summary>
        /// Compression perpendicular to grain Fc⊥.
        /// </summary>
        public double CompressionPerpendicular { get; set; }

        /// <summary>
        /// Shear parallel to grain Fv.
        /// </summary>
        public double ShearStrength { get; set; }

        /// <summary>
        /// Optional grade stamp (e.g. "#2", "Select Structural").
        /// </summary>
        public string? Grade { get; set; }
    }
}
