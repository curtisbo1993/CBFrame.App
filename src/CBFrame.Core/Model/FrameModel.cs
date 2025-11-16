using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;
using System.Collections.Generic;
using System.Xml.Linq;
using CBFrame.Core.Elements;
using CBFrame.Core.Materials;
using CBFrame.Core.Sections;
using CBFrame.Core.Loads;
using CBFrame.Core.Results;

namespace CBFrame.Core.Model
{
    /// <summary>
    /// Root model for a cb_FRAME project.
    /// For Phase 2 Step 1 this is just a container for nodes and basic metadata.
    /// </summary>
    public class FrameModel
    {
        /// <summary>
        /// Display name of the model (e.g. "Office Building - Base Model").
        /// </summary>
        public string Name { get; set; } = "Untitled Model";

        /// <summary>
        /// Optional description of the model or project.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Unit system for this model (e.g. Imperial or Metric).
        /// </summary>
        public UnitSystem UnitSystem { get; set; } = UnitSystem.Imperial;

        /// <summary>
        /// Nodes that define the geometry of the structure.
        /// </summary>
        public IList<Node> Nodes { get; } = [];

        /// <summary>
        /// Elements that connect nodes and carry loads.
        /// </summary>
        public IList<ElementBase> Elements { get; } = new List<ElementBase>();

        /// <summary>
        /// Materials available in this model.
        /// </summary>
        public IList<MaterialBase> Materials { get; } = new List<MaterialBase>();

        /// <summary>
        /// Sections (cross-sections) available in this model.
        /// </summary>
        public IList<SectionBase> Sections { get; } = new List<SectionBase>();

        /// <summary>
        /// Load cases defined in this model.
        /// </summary>
        public IList<LoadCase> LoadCases { get; } = new List<LoadCase>();

        /// <summary>
        /// Load combinations for design and results.
        /// </summary>
        public IList<LoadCombination> LoadCombinations { get; } = new List<LoadCombination>();

        /// <summary>
        /// Nodal loads applied to nodes.
        /// </summary>
        public IList<NodalLoad> NodalLoads { get; } = new List<NodalLoad>();

        /// <summary>
        /// Line loads applied to line elements.
        /// </summary>
        public IList<LineLoad> LineLoads { get; } = new List<LineLoad>();

        /// <summary>
        /// Node displacement results.
        /// </summary>
        public IList<NodeResult> NodeResults { get; } = new List<NodeResult>();

        /// <summary>
        /// Element force and moment results.
        /// </summary>
        public IList<ElementResult> ElementResults { get; } = new List<ElementResult>();

        /// <summary>
        /// Support reaction results at restrained nodes.
        /// </summary>
        public IList<SupportReaction> SupportReactions { get; } = new List<SupportReaction>();

        // NOTE:
        // We will add elements, loads, materials, sections, and results later
        // in this phase to avoid breaking the build at each step.
    }
}
