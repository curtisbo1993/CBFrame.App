using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf.Tools
{
    public interface ITool
    {
        string Name { get; }

        void OnLeftClick(Point3D worldPosition);
        void OnMouseMove(Point3D worldPosition);
        void OnCancel();
    }
}
