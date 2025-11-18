using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf.Services
{
    public interface IModelEditingService
    {
        void AddNode(Point3D position);
        void AddMember(Point3D start, Point3D end);
    }
}
