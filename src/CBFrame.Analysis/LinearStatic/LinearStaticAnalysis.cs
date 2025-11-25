using System;
using System.Collections.Generic;

namespace CBFrame.Analysis.LinearStatic
{
    public sealed class LinearStaticAnalysis
    {
        private readonly IReadOnlyList<FrameElement2D> _elements;
        private readonly DofMapper _dofMapper;

        public LinearStaticAnalysis(IReadOnlyList<FrameElement2D> elements, DofMapper dofMapper)
        {
            _elements = elements ?? throw new ArgumentNullException(nameof(elements));
            _dofMapper = dofMapper ?? throw new ArgumentNullException(nameof(dofMapper));
        }

        public double[] Solve(double[] globalForces)
        {
            int n = _dofMapper.FreeDofCount;
            if (globalForces.Length != n)
                throw new ArgumentException("Force vector size must equal free DOF count.");

            double[,] K = new double[n, n];
            double[] F = (double[])globalForces.Clone();

            foreach (var elem in _elements)
            {
                var ke = elem.GetGlobalStiffness();
                var dofs = _dofMapper.GetElementDofIndices(elem.NodeIIndex, elem.NodeJIndex);

                for (int r = 0; r < 6; r++)
                {
                    int globalR = dofs[r];
                    if (globalR < 0) continue;

                    for (int c = 0; c < 6; c++)
                    {
                        int globalC = dofs[c];
                        if (globalC < 0) continue;

                        K[globalR, globalC] += ke[r, c];
                    }
                }
            }

            return SolveLinearSystem(K, F);
        }

        private static double[] SolveLinearSystem(double[,] A, double[] b)
        {
            int n = b.Length;
            double[,] m = (double[,])A.Clone();
            double[] x = (double[])b.Clone();

            // Forward elimination
            for (int i = 0; i < n; i++)
            {
                double pivot = m[i, i];
                if (Math.Abs(pivot) < 1e-12)
                    throw new InvalidOperationException("Singular matrix in analysis.");

                for (int j = i; j < n; j++)
                    m[i, j] /= pivot;

                x[i] /= pivot;

                for (int r = i + 1; r < n; r++)
                {
                    double factor = m[r, i];

                    for (int c = i; c < n; c++)
                        m[r, c] -= factor * m[i, c];

                    x[r] -= factor * x[i];
                }
            }

            // Back substitution
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = i + 1; j < n; j++)
                    x[i] -= m[i, j] * x[j];
            }

            return x;
        }
    }
}
