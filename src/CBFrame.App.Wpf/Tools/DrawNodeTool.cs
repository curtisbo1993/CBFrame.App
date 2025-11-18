using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Services;

namespace CBFrame.App.Wpf.Tools
{
    public class DrawNodeTool : ITool
    {
        private readonly IModelEditingService _modelEditingService;

        public string Name => "Draw Node";

        public DrawNodeTool(IModelEditingService modelEditingService)
        {
            _modelEditingService = modelEditingService;
        }

        public void OnLeftClick(Point3D worldPosition)
        {
            // For now: simply create a node at the clicked position.
            // This will go through UndoRedoService via ModelEditingService.
            _modelEditingService.AddNode(worldPosition);
        }

        public void OnMouseMove(Point3D worldPosition)
        {
            // Later we can add a preview or snapping here.
        }

        public void OnCancel()
        {
            // Nothing to cancel for single-click nodes (yet).
        }
    }
}
