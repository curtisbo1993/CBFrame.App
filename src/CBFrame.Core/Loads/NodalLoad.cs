using CBFrame.Core.Enums;
using CBFrame.Core.Geometry;

namespace CBFrame.Core.Loads
{
    public sealed class NodalLoad
    {
        public int NodeId { get; set; }

        // Force components in global axes
        public double Fx { get; set; }
        public double Fy { get; set; }
        public double Fz { get; set; }

        // Moment components
        public double Mx { get; set; }
        public double My { get; set; }
        public double Mz { get; set; }

        /// <summary>
        /// The load case this load belongs to.
        /// </summary>
        public int LoadCaseId { get; set; }

        public NodalLoad(int nodeId, int loadCaseId)
        {
            NodeId = nodeId;
            LoadCaseId = loadCaseId;
        }
    }
}
