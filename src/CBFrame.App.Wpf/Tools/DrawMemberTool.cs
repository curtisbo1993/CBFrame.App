using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Services;

namespace CBFrame.App.Wpf.Tools
{
    public class DrawMemberTool : ITool
    {
        private readonly IModelEditingService _modelEditingService;

        // First click = start point, second click = end point.
        private Point3D? _pendingStartPoint;

        public string Name => "Draw Member";

        public DrawMemberTool(IModelEditingService modelEditingService)
        {
            _modelEditingService = modelEditingService;
        }

        public void OnLeftClick(Point3D worldPosition)
        {
            if (_pendingStartPoint == null)
            {
                // First click: remember the start point.
                _pendingStartPoint = worldPosition;
            }
            else
            {
                // Second click: create a member from start to current point.
                _modelEditingService.AddMember(_pendingStartPoint.Value, worldPosition);

                // Reset so a new member can be started.
                _pendingStartPoint = null;
            }
        }

        public void OnMouseMove(Point3D worldPosition)
        {
            // Later we can add a "rubber-band" preview from _pendingStartPoint to the mouse.
        }

        public void OnCancel()
        {
            // Cancel any in-progress member.
            _pendingStartPoint = null;
        }
    }
}
