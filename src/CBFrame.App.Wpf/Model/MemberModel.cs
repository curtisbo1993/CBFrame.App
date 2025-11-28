using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf.Model
{
    /// <summary>
    /// Simple member between two points.
    /// For now it references raw points; later we can hook it to nodes.
    /// </summary>
    public class MemberModel
    {
        public int Id { get; }
        public Point3D Start { get; set; }
        public Point3D End { get; set; }

        /// <summary>
        /// Section identifier (e.g. "W10X33") chosen from the sections database.
        /// </summary>
        public string? SectionId { get; set; }

        /// <summary>
        /// Material identifier (e.g. "ASTM_A992") chosen from the materials database.
        /// </summary>
        public string? MaterialId { get; set; }

        public MemberModel(int id, Point3D start, Point3D end)
        {
            Id = id;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"Member {Id} [{Start.X:0.##},{Start.Y:0.##},{Start.Z:0.##}] → " +
                   $"[{End.X:0.##},{End.Y:0.##},{End.Z:0.##}]";
        }
    }
}
