using System;
using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.Services;

namespace CBFrame.App.Wpf.Tools
{
    public class SelectTool : ITool
    {
        private readonly ISelectionService _selectionService;
        private readonly FrameDocument _document;

        public string Name => "Select";

        public SelectTool(ISelectionService selectionService, FrameDocument document)
        {
            _selectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public void OnLeftClick(Point3D worldPosition)
        {
            // 1) Try to pick a node near the click
            var node = HitTestNode(worldPosition);
            if (node != null)
            {
                _selectionService.SetSelection(node);
                return;
            }

            // 2) Try to pick a member near the click
            var member = HitTestMember(worldPosition);
            if (member != null)
            {
                _selectionService.SetSelection(member);
                return;
            }

            // 3) Nothing hit: clear selection
            _selectionService.ClearSelection();
        }

        public void OnMouseMove(Point3D worldPosition)
        {
            // Later we can show hover previews if we want.
        }

        public void OnCancel()
        {
            // Nothing special to cancel for Select.
        }

        // ----- Hit-testing helpers -----

        private NodeModel? HitTestNode(Point3D p)
        {
            const double radius = 8.0;          // pixels
            double radiusSq = radius * radius;

            NodeModel? best = null;
            double bestDistSq = double.MaxValue;

            foreach (var node in _document.Nodes)
            {
                double dx = node.Position.X - p.X;
                double dy = node.Position.Y - p.Y;
                double distSq = dx * dx + dy * dy;

                if (distSq <= radiusSq && distSq < bestDistSq)
                {
                    bestDistSq = distSq;
                    best = node;
                }
            }

            return best;
        }

        private MemberModel? HitTestMember(Point3D p)
        {
            const double tolerance = 6.0; // pixels

            foreach (var member in _document.Members)
            {
                double dist = DistancePointToSegment2D(
                    p,
                    member.Start,
                    member.End);

                if (dist <= tolerance)
                {
                    return member;
                }
            }

            return null;
        }

        private static double DistancePointToSegment2D(Point3D p, Point3D a, Point3D b)
        {
            // Work in 2D (ignore Z)
            double ax = a.X;
            double ay = a.Y;
            double bx = b.X;
            double by = b.Y;
            double px = p.X;
            double py = p.Y;

            double dx = bx - ax;
            double dy = by - ay;
            double lengthSq = dx * dx + dy * dy;

            if (lengthSq < 1e-6)
            {
                // Segment is extremely short; treat as a point.
                double dxp = px - ax;
                double dyp = py - ay;
                return Math.Sqrt(dxp * dxp + dyp * dyp);
            }

            double t = ((px - ax) * dx + (py - ay) * dy) / lengthSq;
            t = Math.Clamp(t, 0.0, 1.0);

            double closestX = ax + t * dx;
            double closestY = ay + t * dy;

            double ddx = px - closestX;
            double ddy = py - closestY;

            return Math.Sqrt(ddx * ddx + ddy * ddy);
        }
    }
}
