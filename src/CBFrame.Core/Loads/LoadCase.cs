using System.Numerics;

namespace CBFrame.Core.Loads
{
    public sealed class LoadCase
    {
        public int Id { get; }
        public string Name { get; set; }
        public LoadCaseType Type { get; set; }

        /// <summary>
        /// Whether this case includes self-weight (gravity) from member/plate masses.
        /// </summary>
        public bool IncludeSelfWeight { get; set; }

        /// <summary>
        /// Gravity vector direction (e.g. (0, 0, -1) for vertical -Z).
        /// Magnitude is typically 1; the solver multiplies by g, etc.
        /// </summary>
        public Vector3 GravityDirection { get; set; } = new Vector3(0, 0, -1);

        /// <summary>
        /// Scale factor applied to self-weight loads (e.g. 1.0, or 0.9 in combos).
        /// For the raw case, this is usually 1.0.
        /// </summary>
        public double SelfWeightFactor { get; set; } = 1.0;

        /// <summary>
        /// If false, analysis can skip this case.
        /// </summary>
        public bool IsActive { get; set; } = true;

        public LoadCase(int id, string name, LoadCaseType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public override string ToString() => $"{Id}: {Name} ({Type})";
    }
}
