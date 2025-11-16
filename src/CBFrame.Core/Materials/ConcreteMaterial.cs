using CBFrame.Core.Enums;

namespace CBFrame.Core.Materials
{
    /// <summary>
    /// Concrete material definition.
    /// Phase 2: data only, no code checks.
    /// </summary>
    public class ConcreteMaterial : MaterialBase
    {
        public ConcreteMaterial()
        {
            MaterialType = MaterialType.Concrete;
        }

        /// <summary>
        /// Specified compressive strength f'c.
        /// </summary>
        public double CompressiveStrength { get; set; }

        /// <summary>
        /// Lightweight vs normal weight, etc.
        /// </summary>
        public ConcreteWeightClass WeightClass { get; set; } = ConcreteWeightClass.NormalWeight;

        /// <summary>
        /// Optional exposure or durability class label.
        /// </summary>
        public string? ExposureClass { get; set; }
    }
}
