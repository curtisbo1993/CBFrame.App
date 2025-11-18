using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Undoable command that adds a node to the frame document.
    /// </summary>
    public class AddNodeCommand : IUndoableCommand
    {
        private readonly FrameDocument _document;
        private readonly Point3D _position;

        private NodeModel? _createdNode;

        public string Name => "Add Node";

        public AddNodeCommand(FrameDocument document, Point3D position)
        {
            _document = document;
            _position = position;
        }

        public void Execute()
        {
            if (_createdNode == null)
            {
                _createdNode = _document.CreateNode(_position);
            }
            else
            {
                // Redo: reinsert existing node
                _document.Nodes.Add(_createdNode);
            }
        }

        public void Undo()
        {
            if (_createdNode != null)
            {
                _document.RemoveNode(_createdNode);
            }
        }

        public NodeModel? CreatedNode => _createdNode;
    }
}
