using System;

namespace CBFrame.Analysis.LinearStatic
{
    public sealed class FrameElement2D
    {
        public int NodeIIndex { get; }
        public int NodeJIndex { get; }

        public double Xi { get; }
        public double Zi { get; }
        public double Xj { get; }
        public double Zj { get; }

        public double EA { get; }
        public double EI { get; }

        public double Length { get; }
        public double CosX { get; }
        public double CosZ { get; }

        public FrameElement2D(
            int nodeIIndex,
            int nodeJIndex,
            double xi,
            double zi,
            double xj,
            double zj,
            double ea,
            double ei)
        {
            if (nodeIIndex < 0) throw new ArgumentOutOfRangeException(nameof(nodeIIndex));
            if (nodeJIndex < 0) throw new ArgumentOutOfRangeException(nameof(nodeJIndex));
            if (ea <= 0.0) throw new ArgumentOutOfRangeException(nameof(ea), "EA must be positive.");
            if (ei <= 0.0) throw new ArgumentOutOfRangeException(nameof(ei), "EI must be positive.");

            NodeIIndex = nodeIIndex;
            NodeJIndex = nodeJIndex;

            Xi = xi;
            Zi = zi;
            Xj = xj;
            Zj = zj;

            EA = ea;
            EI = ei;

            double dx = Xj - Xi;
            double dz = Zj - Zi;
            double length = Math.Sqrt(dx * dx + dz * dz);

            if (length <= 0.0)
                throw new InvalidOperationException("Element length must be positive.");

            Length = length;
            CosX = dx / length;
            CosZ = dz / length;
        }

        public double[,] GetLocalStiffness()
        {
            double L = Length;
            double L2 = L * L;
            double L3 = L2 * L;

            double EAoverL = EA / L;
            double EIoverL3 = EI * 12.0 / L3;
            double EIoverL2 = EI * 6.0 / L2;
            double EIoverL = EI * 4.0 / L;
            double EIoverL_half = EI * 2.0 / L;

            var k = new double[6, 6];

            // Axial terms
            k[0, 0] = EAoverL;
            k[0, 3] = -EAoverL;
            k[3, 0] = -EAoverL;
            k[3, 3] = EAoverL;

            // Bending terms
            k[1, 1] = EIoverL3;
            k[1, 2] = EIoverL2;
            k[1, 4] = -EIoverL3;
            k[1, 5] = EIoverL2;

            k[2, 1] = EIoverL2;
            k[2, 2] = EIoverL;
            k[2, 4] = -EIoverL2;
            k[2, 5] = EIoverL_half;

            k[4, 1] = -EIoverL3;
            k[4, 2] = -EIoverL2;
            k[4, 4] = EIoverL3;
            k[4, 5] = -EIoverL2;

            k[5, 1] = EIoverL2;
            k[5, 2] = EIoverL_half;
            k[5, 4] = -EIoverL2;
            k[5, 5] = EIoverL;

            return k;
        }

        public double[,] GetGlobalStiffness()
        {
            double[,] kLocal = GetLocalStiffness();
            double[,] T = BuildTransformationMatrix(CosX, CosZ);

            var temp = Multiply6x6(T, kLocal);
            var tTranspose = Transpose6x6(T);
            var kGlobal = Multiply6x6(temp, tTranspose);

            return kGlobal;
        }

        private static double[,] BuildTransformationMatrix(double c, double s)
        {
            var T = new double[6, 6];

            // Node 1
            T[0, 0] = c; T[0, 1] = -s;
            T[1, 0] = s; T[1, 1] = c;
            T[2, 2] = 1.0;

            // Node 2
            T[3, 3] = c; T[3, 4] = -s;
            T[4, 3] = s; T[4, 4] = c;
            T[5, 5] = 1.0;

            return T;
        }

        private static double[,] Multiply6x6(double[,] a, double[,] b)
        {
            var result = new double[6, 6];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < 6; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        private static double[,] Transpose6x6(double[,] m)
        {
            var t = new double[6, 6];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    t[i, j] = m[j, i];
                }
            }

            return t;
        }
    }
}
