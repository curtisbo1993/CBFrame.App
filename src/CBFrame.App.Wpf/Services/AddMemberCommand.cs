using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Undoable command that adds a member to the frame document.
    /// </summary>
    public class AddMemberCommand : IUndoableCommand
    {
        private readonly FrameDocument _document;
        private readonly Point3D _start;
        private readonly Point3D _end;

        private MemberModel? _createdMember;

        public string Name => "Add Member";

        public AddMemberCommand(FrameDocument document, Point3D start, Point3D end)
        {
            _document = document;
            _start = start;
            _end = end;
        }

        public void Execute()
        {
            if (_createdMember == null)
            {
                _createdMember = _document.CreateMember(_start, _end);
            }
            else
            {
                // Redo: reinsert existing member
                _document.Members.Add(_createdMember);
            }
        }

        public void Undo()
        {
            if (_createdMember != null)
            {
                _document.RemoveMember(_createdMember);
            }
        }

        public MemberModel? CreatedMember => _createdMember;
    }
}
