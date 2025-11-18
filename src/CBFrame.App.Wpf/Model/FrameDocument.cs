using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf.Model
{
    /// <summary>
    /// In-memory representation of the current frame model.
    /// </summary>
    public class FrameDocument
    {
        private int _nextNodeId = 1;
        private int _nextMemberId = 1;

        public ObservableCollection<NodeModel> Nodes { get; } = new();
        public ObservableCollection<MemberModel> Members { get; } = new();

        public NodeModel CreateNode(Point3D position)
        {
            var node = new NodeModel(_nextNodeId++, position);
            Nodes.Add(node);
            return node;
        }

        public void RemoveNode(NodeModel node)
        {
            Nodes.Remove(node);
        }

        public MemberModel CreateMember(Point3D start, Point3D end)
        {
            var member = new MemberModel(_nextMemberId++, start, end);
            Members.Add(member);
            return member;
        }

        public void RemoveMember(MemberModel member)
        {
            Members.Remove(member);
        }
    }
}
