using System;
using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;
using CBFrame.IO.Serialization;

namespace CBFrame.App.Wpf.Services;

/// <summary>
/// Default implementation of IProjectFileService.
/// Maps between FrameDocument and the .cbf DTOs,
/// and delegates JSON work to CbfSerializer/CbfDeserializer.
/// </summary>
public sealed class ProjectFileService : IProjectFileService
{
    private readonly CbfSerializer _serializer;
    private readonly CbfDeserializer _deserializer;

    public ProjectFileService()
    {
        _serializer = new CbfSerializer();
        _deserializer = new CbfDeserializer();
    }

    public FrameDocument CreateNewDocument()
    {
        // Your FrameDocument probably sets up Nodes/Members internally.
        // We don't try to set Name/Description here because they don't exist yet.
        return new FrameDocument();
    }

    public void SaveToFile(string filePath, FrameDocument document)
    {
        if (document is null) throw new ArgumentNullException(nameof(document));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path must not be null or empty.", nameof(filePath));

        var root = MapDocumentToCbf(document);
        _serializer.SerializeToFile(root, filePath);
    }

    public FrameDocument LoadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path must not be null or empty.", nameof(filePath));

        var root = _deserializer.DeserializeFromFile(filePath);
        return MapCbfToDocument(root);
    }

    // ---------------------------------------------------------------------
    // Mapping: FrameDocument -> CbfProjectRoot
    // ---------------------------------------------------------------------

    private static CbfProjectRoot MapDocumentToCbf(FrameDocument document)
    {
        var root = new CbfProjectRoot
        {
            CbfVersion = 1,
            Project = new CbfProjectDto
            {
                // We don't have Name/Description on FrameDocument yet,
                // so we just put some defaults for now.
                Name = "Untitled",
                Description = string.Empty
            }
        };

        // ---- Nodes ----
        foreach (var node in document.Nodes)
        {
            // ASSUMPTION: NodeModel has a Position property of type Point3D.
            // If your property name is different, change "Position" accordingly.
            Point3D p = node.Position;

            var nodeDto = new CbfNodeDto
            {
                Id = node.Id,
                X = p.X,
                Y = p.Y,
                Z = p.Z,

                // You don't appear to have restraints on NodeModel yet,
                // so we'll leave this null for now.
                Restraint = null
            };

            root.Project.Nodes.Add(nodeDto);
        }

        // ---- Members ----
        foreach (var member in document.Members)
        {
            // MemberModel exposes its end points as Start and End of type Point3D.
            Point3D pi = member.Start;
            Point3D pj = member.End;

            var memberDto = new CbfMemberDto
            {
                Id = member.Id,

                IX = pi.X,
                IY = pi.Y,
                IZ = pi.Z,

                JX = pj.X,
                JY = pj.Y,
                JZ = pj.Z
            };

            root.Project.Members.Add(memberDto);
        }

        return root;
    }

    // ---------------------------------------------------------------------
    // Mapping: CbfProjectRoot -> FrameDocument
    // ---------------------------------------------------------------------

    private static FrameDocument MapCbfToDocument(CbfProjectRoot root)
    {
        if (root.Project is null)
            throw new InvalidOperationException("CBF project payload is missing.");

        var project = root.Project;

        var document = new FrameDocument();

        // Clear any existing model state.
        document.Nodes.Clear();
        document.Members.Clear();

        // ---- Nodes ----
        foreach (var nodeDto in project.Nodes)
        {
            var position = new Point3D(nodeDto.X, nodeDto.Y, nodeDto.Z);
            var node = new NodeModel(nodeDto.Id, position);

            document.Nodes.Add(node);
        }

        // ---- Members ----
        foreach (var memberDto in project.Members)
        {
            var iPos = new Point3D(memberDto.IX, memberDto.IY, memberDto.IZ);
            var jPos = new Point3D(memberDto.JX, memberDto.JY, memberDto.JZ);

            var member = new MemberModel(memberDto.Id, iPos, jPos);

            document.Members.Add(member);
        }

        return document;
    }
}
