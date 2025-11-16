using CBFrame.Core.Elements;
using CBFrame.Core.Enums;
using CBFrame.Core.Materials;
using CBFrame.Core.Sections;

namespace CBFrame.Core.Geometry
{
    /// <summary>
    /// A geometric node in 3D space.
    /// For Phase 2 Step 1 this only stores coordinates and simple support flags.
    /// No analysis logic is implemented here.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Unique identifier for the node within the model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// X-coordinate of the node (based on model units).
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y-coordinate of the node (based on model units).
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z-coordinate of the node (based on model units).
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Optional name or label shown in the UI (e.g. "N1").
        /// </summary>
        public string? Label { get; set; }

        // Simple support / restraint flags (no stiffness logic here)

        /// <summary>
        /// Restraint for translation in global X direction.
        /// </summary>
        public bool RestrainUx { get; set; }

        /// <summary>
        /// Restraint for translation in global Y direction.
        /// </summary>
        public bool RestrainUy { get; set; }

        /// <summary>
        /// Restraint for translation in global Z direction.
        /// </summary>
        public bool RestrainUz { get; set; }

        /// <summary>
        /// Restraint for rotation about global X axis.
        /// </summary>
        public bool RestrainRx { get; set; }

        /// <summary>
        /// Restraint for rotation about global Y axis.
        /// </summary>
        public bool RestrainRy { get; set; }

        /// <summary>
        /// Restraint for rotation about global Z axis.
        /// </summary>
        public bool RestrainRz { get; set; }

        /// <summary>
        /// Optional support type classification (Pinned, Fixed, Roller, etc.).
        /// This is descriptive only in Phase 2, no behavior.
        /// </summary>
        public SupportType SupportType { get; set; } = SupportType.Custom;
    }
}
