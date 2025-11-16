using CBFrame.Core.Enums;

namespace CBFrame.Core.Sections
{
    /// <summary>
    /// Base class for all cross-sections.
    /// Phase 2: geometry properties only, no calculations.
    /// </summary>
    public abstract class SectionBase
    {
        /// <summary>
        /// Unique ID of the section.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Human-readable name / description (e.g. "W14x90", "24x30 Slab").
        /// </summary>
        public string Name { get; set; } = "Unnamed Section";

        /// <summary>
        /// General section type (Steel, Concrete, Wood, etc.).
        /// </summary>
        public SectionMaterialType SectionMaterialType { get; protected set; } = SectionMaterialType.Unknown;

        /// <summary>
        /// Shape family (W-shape, channel, rectangle, etc.).
        /// </summary>
        public SectionShapeFamily ShapeFamily { get; set; } = SectionShapeFamily.Unknown;

        /// <summary>
        /// Gross area A.
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// Moment of inertia about local 2-2 axis (Iy).
        /// </summary>
        public double InertiaMinor { get; set; }

        /// <summary>
        /// Moment of inertia about local 3-3 axis (Iz).
        /// </summary>
        public double InertiaMajor { get; set; }

        /// <summary>
        /// Torsional constant J.
        /// </summary>
        public double TorsionalConstant { get; set; }

        /// <summary>
        /// Shear area in local 2 direction.
        /// </summary>
        public double ShearArea2 { get; set; }

        /// <summary>
        /// Shear area in local 3 direction.
        /// </summary>
        public double ShearArea3 { get; set; }
    }
}
