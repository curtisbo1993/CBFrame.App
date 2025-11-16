using CBFrame.Core.Enums;

namespace CBFrame.Core.Materials
{
    /// <summary>
    /// Steel material definition.
    /// Phase 2: only holds basic strength and classification values.
    /// </summary>
    public class SteelMaterial : MaterialBase
    {
        public SteelMaterial()
        {
            MaterialType = MaterialType.Steel;
        }

        /// <summary>
        /// Yield strength Fy.
        /// </summary>
        public double YieldStrength { get; set; }

        /// <summary>
        /// Ultimate strength Fu.
        /// </summary>
        public double UltimateStrength { get; set; }

        /// <summary>
        /// Optional steel grade classification (e.g. A992, A36).
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// Optional shape production type (rolled, built-up, etc.).
        /// </summary>
        public SteelShapeType ShapeType { get; set; } = SteelShapeType.Unknown;
    }
}
