namespace CBFrame.Core.Materials
{
    /// <summary>
    /// Basic material category used for filters and code logic.
    /// </summary>
    public enum MaterialKind
    {
        Steel,
        Concrete,
        Wood,
        Masonry,
        Aluminum,
        Other
    }

    /// <summary>
    /// Material definition used by the analysis engine and design modules.
    /// These values should align with code-based material tables
    /// (AISC, ACI, NDS, etc.).
    /// </summary>
    public sealed class MaterialDefinition
    {
        /// <summary>
        /// Unique ID for this material (e.g. "STEEL_A992_FY50").
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable name (e.g. "A992 Steel Fy=50 ksi").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category (Steel, Concrete, Wood, etc.).
        /// </summary>
        public MaterialKind Kind { get; set; }

        /// <summary>
        /// Optional grade or spec (e.g. "ASTM A992", "A615 Grade 60").
        /// </summary>
        public string Grade { get; set; } = string.Empty;

        // Basic mechanical properties.
        // Choose consistent units; here we'll assume:
        // E, G, Fy, Fu in ksi; Density in kips/ft^3;

        public double E { get; set; }          // ksi
        public double G { get; set; }          // ksi
        public double Density { get; set; }    // kips/ft^3 (or whatever convention you settle on)
        public double Poisson { get; set; }    // dimensionless

        /// <summary>
        /// Yield strength (ksi). 0 for materials without a defined Fy.
        /// </summary>
        public double Fy { get; set; }

        /// <summary>
        /// Ultimate strength (ksi). 0 if not applicable.
        /// </summary>
        public double Fu { get; set; }
    }
}
