using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf.Model
{
    /// <summary>
    /// Simple node in the frame model.
    /// </summary>
    public class NodeModel
    {
        public int Id { get; }
        public Point3D Position { get; set; }

        public NodeModel(int id, Point3D position)
        {
            Id = id;
            Position = position;
        }

        public override string ToString()
        {
            return $"Node {Id} ({Position.X:0.##}, {Position.Y:0.##}, {Position.Z:0.##})";
        }
    }
}
