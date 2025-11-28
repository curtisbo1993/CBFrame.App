namespace CBFrame.Core.Sections
{
    /// <summary>
    /// High-level type of cross-section. This is for filtering and UI,
    /// not for exact formulas.
    /// </summary>
    public enum SectionShapeType
    {
        WideFlange,
        Channel,
        Angle,
        DoubleAngle,
        RectangularTube,
        RoundTube,
        SolidRectangle,
        SolidCircle,
        Custom
    }

    /// <summary>
    /// A prismatic section shape from the database (e.g. W12x26).
    /// These values are typically taken directly from a code-based table
    /// like AISC, CISC, etc.
    /// </summary>
    public sealed class SectionShape
    {
        /// <summary>
        /// Unique ID for this shape inside cb_FRAME (e.g. "AISC.W.W12X26").
        /// Use this as the stable key when referencing shapes from members.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable label (e.g. "W12x26").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Shape category (WF, channel, tube, etc.).
        /// </summary>
        public SectionShapeType ShapeType { get; set; }

        /// <summary>
        /// Database or source name (e.g. "AISC", "User", "CSA").
        /// </summary>
        public string Database { get; set; } = string.Empty;

        /// <summary>
        /// Material ID this shape is usually paired with (e.g. "STEEL_A992").
        /// Can be empty if not tied to a default material.
        /// </summary>
        public string DefaultMaterialId { get; set; } = string.Empty;

        // -------------------------
        // Geometric / section props
        // Units are up to you; pick one system and stay consistent.
        // For now assume: inches, in^2, in^4, in^3, lb/ft.
        // -------------------------

        public double Area { get; set; }           // in^2
        public double Ix { get; set; }             // in^4 (strong axis)
        public double Iy { get; set; }             // in^4 (weak axis)
        public double J { get; set; }              // in^4 (torsion)
        public double Sx { get; set; }             // in^3
        public double Sy { get; set; }             // in^3
        public double Zx { get; set; }             // in^3 (plastic modulus, optional)
        public double Zy { get; set; }             // in^3 (plastic modulus, optional)
        public double WeightPerLength { get; set; } // lb/ft

        // Basic overall dimensions (for UI & quick calcs)

        public double Depth { get; set; }          // in (overall depth, d)
        public double FlangeWidth { get; set; }    // in (bf)
        public double WebThickness { get; set; }   // in (tw)
        public double FlangeThickness { get; set; } // in (tf)
    }
}
