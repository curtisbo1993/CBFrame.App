namespace CBFrame.MaterialsDb.Models
{
    /// <summary>
    /// Minimal material record used by the materials database / lookup layer.
    /// </summary>
    public sealed class MaterialRecord
    {
        /// <summary>
        /// Unique material ID used as a key and for section FK links
        /// (e.g. "ASTM_A992").
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Display name (e.g. "A992 Fy=50 ksi").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Material type/category ("Steel", "Concrete", "Wood", etc.).
        /// </summary>
        public string MaterialType { get; set; } = string.Empty;

        /// <summary>
        /// Elastic modulus (E) in consistent units (e.g. ksi).
        /// </summary>
        public double E { get; set; }

        /// <summary>
        /// Shear modulus (G) in consistent units (e.g. ksi).
        /// </summary>
        public double G { get; set; }

        /// <summary>
        /// Density (e.g. kcf or kips/ft³ depending on your convention).
        /// </summary>
        public double Density { get; set; }

        /// <summary>
        /// Yield strength Fy (ksi), if applicable.
        /// </summary>
        public double Fy { get; set; }

        /// <summary>
        /// Ultimate strength Fu (ksi), if applicable.
        /// </summary>
        public double Fu { get; set; }
    }
}
