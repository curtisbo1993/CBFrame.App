using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Elements
{
    /// <summary>
    /// Base class for all structural elements.
    /// No analysis logic in Phase 2 — only properties.
    /// </summary>
    public abstract class ElementBase
    {
        /// <summary>
        /// Unique ID of the element.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Optional label (e.g. "B1", "C3").
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Type of structural element.
        /// </summary>
        public ElementType ElementType { get; protected set; } = ElementType.Unknown;

        /// <summary>
        /// Start node of the element.
        /// </summary>
        public Node INode { get; set; } = null!;

        /// <summary>
        /// End node of the element.
        /// </summary>
        public Node JNode { get; set; } = null!;

        /// <summary>
        /// Material ID (references material in the model).
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Section ID (references section in the model).
        /// </summary>
        public int SectionId { get; set; }
    }
}
