using System;
using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.Tools;

namespace CBFrame.App.Wpf.ViewModels.Panels
{
    public sealed class Model3DViewModel
    {
        private readonly ToolController _toolController;
        private readonly FrameDocument _document;

        public Model3DViewModel(ToolController toolController, FrameDocument document)
        {
            _toolController = toolController ?? throw new ArgumentNullException(nameof(toolController));
            _document = document ?? throw new ArgumentNullException(nameof(document));

            // Later we'll use _document.Nodes and _document.Members
            // to build 3D visuals.
        }

        public FrameDocument Document => _document;


        /// <summary>
        /// Called by the 3D view when the user left-clicks in the viewport.
        /// </summary>
        public void OnViewportLeftClick(Point3D worldPosition)
        {
            _toolController.ActiveTool.OnLeftClick(worldPosition);
        }

        /// <summary>
        /// Called by the 3D view when the user moves the mouse over the viewport.
        /// </summary>
        public void OnViewportMouseMove(Point3D worldPosition)
        {
            _toolController.ActiveTool.OnMouseMove(worldPosition);
        }
    }
}
