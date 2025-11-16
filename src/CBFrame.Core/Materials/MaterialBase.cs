using CBFrame.Core.Enums;

namespace CBFrame.Core.Materials
{
    /// <summary>
    /// Base class for all materials.
    /// No design logic in Phase 2 – only properties.
    /// </summary>
    public abstract class MaterialBase
    {
        /// <summary>
        /// Unique ID of the material.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Human-readable name (e.g. "A992 Fy=50 ksi").
        /// </summary>
        public string Name { get; set; } = "Unnamed Material";

        /// <summary>
        /// General material type (Steel, Concrete, Wood, etc.).
        /// </summary>
        public MaterialType MaterialType { get; protected set; } = MaterialType.Unknown;

        /// <summary>
        /// Density (mass per unit volume).
        /// Units depend on model UnitSystem.
        /// </summary>
        public double Density { get; set; }

        /// <summary>
        /// Modulus of elasticity E.
        /// </summary>
        public double ElasticModulus { get; set; }

        /// <summary>
        /// Shear modulus G.
        /// </summary>
        public double ShearModulus { get; set; }

        /// <summary>
        /// Poisson's ratio ν.
        /// </summary>
        public double PoissonRatio { get; set; }

        /// <summary>
        /// Coefficient of thermal expansion α.
        /// </summary>
        public double ThermalExpansionCoeff { get; set; }
    }
}
