using System;

namespace CBFrame.Analysis.LinearStatic
{
    /// <summary>
    /// Maps node degrees of freedom to global equation numbers.
    /// 
    /// Assumes 2D frame:
    /// - 3 DOFs per node: Ux, Uz, ThetaY
    /// - Supports are provided as a bool[3] per node
    ///   where true = restrained (fixed), false = free.
    /// </summary>
    public sealed class DofMapper
    {
        private const int DofsPerNode = 3;

        private readonly int _nodeCount;
        private readonly int[] _globalIndex;
        private readonly int _freeDofCount;

        /// <summary>
        /// Total number of free DOFs.
        /// This determines the size of the global K and d vectors (K is n x n).
        /// </summary>
        public int FreeDofCount => _freeDofCount;

        /// <summary>
        /// Creates a DOF mapper for the given number of nodes.
        /// </summary>
        /// <param name="nodeCount">Number of nodes in the model.</param>
        /// <param name="restraintProvider">
        /// Function that returns a bool[3] for each node index i (0-based),
        /// indicating which DOFs are restrained (true = fixed).
        /// Order of DOFs in the bool[] must be: Ux, Uz, ThetaY.
        /// </param>
        public DofMapper(int nodeCount, Func<int, bool[]> restraintProvider)
        {
            if (nodeCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(nodeCount), "Node count must be positive.");

            _nodeCount = nodeCount;

            if (restraintProvider == null)
                throw new ArgumentNullException(nameof(restraintProvider));

            _globalIndex = new int[_nodeCount * DofsPerNode];

            int currentIndex = 0;

            // Loop over all nodes and DOFs, assigning global equation numbers
            for (int node = 0; node < _nodeCount; node++)
            {
                bool[] restraints = restraintProvider(node);
                if (restraints == null || restraints.Length != DofsPerNode)
                {
                    throw new InvalidOperationException(
                        $"Restraint array for node {node} must be length {DofsPerNode}.");
                }

                for (int dof = 0; dof < DofsPerNode; dof++)
                {
                    int flatIndex = node * DofsPerNode + dof;

                    if (restraints[dof])
                    {
                        // Fixed DOF: mark as -1 (no equation number)
                        _globalIndex[flatIndex] = -1;
                    }
                    else
                    {
                        // Free DOF: assign next global index
                        _globalIndex[flatIndex] = currentIndex;
                        currentIndex++;
                    }
                }
            }

            _freeDofCount = currentIndex;
        }

        /// <summary>
        /// Returns the global equation index for the given node and DOF type,
        /// or -1 if that DOF is restrained.
        /// </summary>
        public int GetGlobalIndex(int nodeIndex, DofType dofType)
        {
            ValidateNodeIndex(nodeIndex);

            int dof = (int)dofType;
            int flatIndex = nodeIndex * DofsPerNode + dof;
            return _globalIndex[flatIndex];
        }

        /// <summary>
        /// Returns the global equation index for the given node and DOF (0..2),
        /// or -1 if that DOF is restrained.
        /// </summary>
        public int GetGlobalIndex(int nodeIndex, int dofIndex)
        {
            ValidateNodeIndex(nodeIndex);

            if (dofIndex < 0 || dofIndex >= DofsPerNode)
                throw new ArgumentOutOfRangeException(nameof(dofIndex));

            int flatIndex = nodeIndex * DofsPerNode + dofIndex;
            return _globalIndex[flatIndex];
        }

        /// <summary>
        /// Returns the 6 global DOF indices for a 2D frame element
        /// that connects (nodeI, nodeJ) in the order:
        /// [i.Ux, i.Uz, i.ThetaY, j.Ux, j.Uz, j.ThetaY].
        /// 
        /// Any restrained DOFs will have index -1.
        /// </summary>
        public int[] GetElementDofIndices(int nodeI, int nodeJ)
        {
            ValidateNodeIndex(nodeI);
            ValidateNodeIndex(nodeJ);

            return new[]
            {
                GetGlobalIndex(nodeI, DofType.Ux),
                GetGlobalIndex(nodeI, DofType.Uz),
                GetGlobalIndex(nodeI, DofType.ThetaY),
                GetGlobalIndex(nodeJ, DofType.Ux),
                GetGlobalIndex(nodeJ, DofType.Uz),
                GetGlobalIndex(nodeJ, DofType.ThetaY),
            };
        }

        private void ValidateNodeIndex(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex >= _nodeCount)
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }
    }
}
