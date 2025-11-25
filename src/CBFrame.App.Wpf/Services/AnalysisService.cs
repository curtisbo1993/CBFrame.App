using System;
using System.Collections.Generic;
using CBFrame.Analysis.LinearStatic;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Result of a linear static analysis.
    /// This is what the UI (view models / views) will consume.
    /// </summary>
    public sealed class AnalysisResult
    {
        /// <summary>
        /// Maps nodes/DOFs to global equation numbers.
        /// The UI will need this to go from displacement vector
        /// back to per-node DOFs.
        /// </summary>
        public DofMapper DofMapper { get; }

        /// <summary>
        /// Elements used in the analysis. The viewport can use this
        /// plus displacements to draw deflected shapes.
        /// </summary>
        public IReadOnlyList<FrameElement2D> Elements { get; }

        /// <summary>
        /// Displacements for all free DOFs, in the same order as
        /// the DofMapper’s equation numbering.
        /// </summary>
        public double[] Displacements { get; }

        public AnalysisResult(
            DofMapper dofMapper,
            IReadOnlyList<FrameElement2D> elements,
            double[] displacements)
        {
            DofMapper = dofMapper ?? throw new ArgumentNullException(nameof(dofMapper));
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
            Displacements = displacements ?? throw new ArgumentNullException(nameof(displacements));
        }
    }

    /// <summary>
    /// High-level service that runs linear static analysis.
    /// 
    /// It is a thin wrapper around the core engine:
    /// CBFrame.Analysis.LinearStatic.LinearStaticAnalysis
    /// </summary>
    public sealed class AnalysisService
    {
        /// <summary>
        /// Runs a 2D linear static analysis given:
        /// - a set of frame elements,
        /// - a DOF mapper,
        /// - and a global nodal force vector (already assembled into free DOFs).
        /// </summary>
        /// <param name="elements">2D frame elements in the model.</param>
        /// <param name="dofMapper">
        /// DOF mapper used to build K and F (must match the elements' node indices).
        /// </param>
        /// <param name="globalForces">
        /// Force vector sized to DofMapper.FreeDofCount (free DOFs only).
        /// </param>
        public AnalysisResult RunLinearStatic(
            IReadOnlyList<FrameElement2D> elements,
            DofMapper dofMapper,
            double[] globalForces)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (dofMapper == null) throw new ArgumentNullException(nameof(dofMapper));
            if (globalForces == null) throw new ArgumentNullException(nameof(globalForces));

            var engine = new LinearStaticAnalysis(elements, dofMapper);
            var displacements = engine.Solve(globalForces);

            return new AnalysisResult(dofMapper, elements, displacements);
        }

        /// <summary>
        /// Convenience helper:
        /// builds the global force vector from simple nodal loads.
        /// 
        /// nodalLoadProvider must return a double[3] for each node:
        /// [Fx, Fz, My] in global coordinates, matching DOF order [Ux, Uz, ThetaY].
        /// </summary>
        public AnalysisResult RunLinearStaticFromNodalLoads(
            IReadOnlyList<FrameElement2D> elements,
            DofMapper dofMapper,
            int nodeCount,
            Func<int, double[]> nodalLoadProvider)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (dofMapper == null) throw new ArgumentNullException(nameof(dofMapper));
            if (nodalLoadProvider == null) throw new ArgumentNullException(nameof(nodalLoadProvider));

            int nFree = dofMapper.FreeDofCount;
            var globalForces = new double[nFree];

            for (int node = 0; node < nodeCount; node++)
            {
                double[] loads = nodalLoadProvider(node);
                if (loads == null || loads.Length != 3)
                    throw new InvalidOperationException("nodalLoadProvider must return double[3] per node.");

                // Map Fx, Fz, My into the global force vector
                for (int localDof = 0; localDof < 3; localDof++)
                {
                    int globalIndex = dofMapper.GetGlobalIndex(node, localDof);
                    if (globalIndex >= 0)
                    {
                        globalForces[globalIndex] += loads[localDof];
                    }
                }
            }

            var engine = new LinearStaticAnalysis(elements, dofMapper);
            var displacements = engine.Solve(globalForces);

            return new AnalysisResult(dofMapper, elements, displacements);
        }
    }
}
