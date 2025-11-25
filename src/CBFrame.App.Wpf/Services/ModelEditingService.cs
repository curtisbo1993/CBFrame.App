using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// High-level editing implementation. Uses undoable commands to modify the frame document.
    /// </summary>
    public class ModelEditingService : IModelEditingService
    {
        private readonly IUndoRedoService _undoRedoService;
        private readonly FrameDocument _document;

        public ModelEditingService(IUndoRedoService undoRedoService, FrameDocument document)
        {
            _undoRedoService = undoRedoService;
            _document = document;
        }

        public void AddNode(Point3D position)
        {
            var cmd = new AddNodeCommand(_document, position);
            _undoRedoService.Push(cmd);
        }

        public void AddMember(Point3D start, Point3D end)
        {
            var cmd = new AddMemberCommand(_document, start, end);
            _undoRedoService.Push(cmd);
        }
        public FrameDocument Document { get; private set; }

        public void SetDocument(FrameDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));

            // Optionally clear selection, reset undo/redo, etc.
        }
    }
}
