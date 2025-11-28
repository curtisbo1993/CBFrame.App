namespace CBFrame.SectionsDb.Models
{
    /// <summary>
    /// Minimal shape record for the sections database.
    /// This is the DB/lookup representation, not the full analysis section.
    /// </summary>
    public sealed class SectionShape
    {
        /// <summary>
        /// Unique ID (e.g. "W10X33").
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Display name (e.g. "W10x33").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Shape family (e.g. "W", "HSS", "C", "L").
        /// </summary>
        public string ShapeType { get; set; } = string.Empty;

        /// <summary>
        /// Cross-section area (in^2 or your chosen units).
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// Strong-axis inertia Ix (in^4).
        /// </summary>
        public double Ix { get; set; }

        /// <summary>
        /// Weak-axis inertia Iy (in^4).
        /// </summary>
        public double Iy { get; set; }

        /// <summary>
        /// Torsional constant J (in^4).
        /// </summary>
        public double J { get; set; }

        /// <summary>
        /// Weight per unit length (e.g. plf).
        /// </summary>
        public double WeightPerLength { get; set; }

        /// <summary>
        /// Foreign key into the materials database (e.g. "ASTM_A992").
        /// </summary>
        public string MaterialId { get; set; } = string.Empty;
    }
}
