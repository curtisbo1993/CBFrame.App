using CBFrame.Analysis.LinearStatic;
using CBFrame.App.Wpf.Services;
using CBFrame.Core.Geometry;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CBFrame.App.Wpf
{
    public enum FrameTool
    {
        None,
        Select,

        // Draw Elements
        Nodes,
        Members,
        Plates,
        WallPanels,
        Solids,
        BoundaryConditions,

        // Draw Loads
        NodalLoad,
        LineLoad,
        PointLoad,
        AreaLoad,
        PlateSurfaceLoad,
        WallSurfaceLoad,

        // Design
        BasicLoadCases,
        LoadCombinations,
        WarningLog,
        SolveTool,

        // Misc
        QuickFind,
        SelectByProperty
    }

    public partial class MainShellWindow : Window
    {
        // =========================
        //  TOOL STATE / ANALYSIS
        // =========================

        private FrameTool _activeTool = FrameTool.None;

        // map each tool enum to its ToggleButton
        private readonly Dictionary<FrameTool, ToggleButton> _toolButtons =
            new Dictionary<FrameTool, ToggleButton>();

        // Phase 6 – Step 5: analysis service for Solve
        private readonly AnalysisService _analysisService = new AnalysisService();

        // =========================
        //  SIMPLE 3D MODEL STATE
        // =========================

        private class Node3D
        {
            public int Id { get; set; }
            public Point3D Position { get; set; }
        }

        private class Member3D
        {
            public int Id { get; set; }

            // Non-nullable ends – set by caller when creating members
            public Node3D Start { get; set; } = null!;
            public Node3D End { get; set; } = null!;
        }

        private readonly List<Node3D> _nodes = new();
        private readonly List<Member3D> _members = new();

        // Viewport visuals (nullable, initialized in InitializeViewportVisuals)
        private ModelVisual3D? _frameVisual;           // undeformed frame (nodes + members)
        private ModelVisual3D? _deflectedShapeVisual;  // reserved for Phase 6 deflected shapes

        // Member drawing state (for FrameTool.Members)
        private Node3D? _pendingMemberStart;

        public MainShellWindow()
        {
            InitializeComponent();

            this.KeyDown += MainWindow_KeyDown;

            // 3D viewport setup
            InitializeViewportVisuals();
            BuildTestModel();
            RenderModelToViewport();
            UpdateStatusCounts();

            // Hook mouse event for drawing/selecting in viewport
            MainViewport.MouseLeftButtonDown += MainViewport_OnMouseLeftButtonDown;

            // wire the toggle buttons AFTER InitializeComponent
            _toolButtons[FrameTool.Nodes] = ToolNodes;
            _toolButtons[FrameTool.Members] = ToolMembers;
            _toolButtons[FrameTool.Plates] = ToolPlates;
            _toolButtons[FrameTool.WallPanels] = ToolWallPanels;
            _toolButtons[FrameTool.Solids] = ToolSolids;
            _toolButtons[FrameTool.BoundaryConditions] = ToolBoundaryConditions;

            _toolButtons[FrameTool.NodalLoad] = ToolNodal;
            _toolButtons[FrameTool.LineLoad] = ToolLine;
            _toolButtons[FrameTool.PointLoad] = ToolPoint;
            _toolButtons[FrameTool.AreaLoad] = ToolArea;
            _toolButtons[FrameTool.PlateSurfaceLoad] = ToolPlateSurface;
            _toolButtons[FrameTool.WallSurfaceLoad] = ToolWallSurface;

            _toolButtons[FrameTool.BasicLoadCases] = ToolBasicLoadCases;
            _toolButtons[FrameTool.LoadCombinations] = ToolLoadCombinations;
            _toolButtons[FrameTool.WarningLog] = ToolWarningLog;
            _toolButtons[FrameTool.SolveTool] = ToolSolve;

            _toolButtons[FrameTool.QuickFind] = ToolQuickFind;
            _toolButtons[FrameTool.SelectByProperty] = ToolSelectByProperty;
        }

        // ✅ Make maximized window respect the taskbar
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;

            // start in Mode: None
            ActivateTool(FrameTool.None);
        }

        // =========================================================
        // ============== HEADER BUTTONS ============================
        // =========================================================

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Help is not implemented yet.",
                "cbFRAME – Help",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void CollapseRibbon_Click(object sender, RoutedEventArgs e)
        {
            // placeholder for ribbon collapse
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized)
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HeaderBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // =========================================================
        // ============== TOOL BUTTONS (ALL RIBBON TOOLS) ==========
        // =========================================================

        private void RibbonTool_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ButtonBase btn)
                return;

            var tool = GetToolFromButton(btn);
            ActivateTool(tool);

            // Phase 6 – Step 5: if this is the Solve tool, run the analysis pipeline
            if (tool == FrameTool.SolveTool)
            {
                RunSolveAnalysis();
            }
        }

        private FrameTool GetToolFromButton(ButtonBase btn)
        {
            // Draw Elements
            if (btn == ToolNodes) return FrameTool.Nodes;
            if (btn == ToolMembers) return FrameTool.Members;
            if (btn == ToolPlates) return FrameTool.Plates;
            if (btn == ToolWallPanels) return FrameTool.WallPanels;
            if (btn == ToolSolids) return FrameTool.Solids;
            if (btn == ToolBoundaryConditions) return FrameTool.BoundaryConditions;

            // Draw Loads
            if (btn == ToolNodal) return FrameTool.NodalLoad;
            if (btn == ToolLine) return FrameTool.LineLoad;
            if (btn == ToolPoint) return FrameTool.PointLoad;
            if (btn == ToolArea) return FrameTool.AreaLoad;
            if (btn == ToolPlateSurface) return FrameTool.PlateSurfaceLoad;
            if (btn == ToolWallSurface) return FrameTool.WallSurfaceLoad;

            // Design
            if (btn == ToolBasicLoadCases) return FrameTool.BasicLoadCases;
            if (btn == ToolLoadCombinations) return FrameTool.LoadCombinations;
            if (btn == ToolWarningLog) return FrameTool.WarningLog;
            if (btn == ToolSolve) return FrameTool.SolveTool;

            // Misc
            if (btn == ToolQuickFind) return FrameTool.QuickFind;
            if (btn == ToolSelectByProperty) return FrameTool.SelectByProperty;

            return FrameTool.None;
        }

        private static string GetToolDisplayName(FrameTool tool)
        {
            switch (tool)
            {
                case FrameTool.None: return "None";
                case FrameTool.Select: return "Select";

                case FrameTool.Nodes: return "Nodes";
                case FrameTool.Members: return "Members";
                case FrameTool.Plates: return "Plates";
                case FrameTool.WallPanels: return "Wall Panels";
                case FrameTool.Solids: return "Solids";
                case FrameTool.BoundaryConditions: return "Boundary Conditions";

                case FrameTool.NodalLoad: return "Nodal";
                case FrameTool.LineLoad: return "Line";
                case FrameTool.PointLoad: return "Point";
                case FrameTool.AreaLoad: return "Area";
                case FrameTool.PlateSurfaceLoad: return "Plate Surface";
                case FrameTool.WallSurfaceLoad: return "Wall Surface";

                case FrameTool.BasicLoadCases: return "Basic Load Cases";
                case FrameTool.LoadCombinations: return "Load Combinations";
                case FrameTool.WarningLog: return "Warning Log";
                case FrameTool.SolveTool: return "Solve";

                case FrameTool.QuickFind: return "Quick Find";
                case FrameTool.SelectByProperty: return "Select by Property";

                default: return tool.ToString();
            }
        }

        // =========================================================
        // ============= OTHER RIBBON BUTTONS (non-tools) ==========
        // =========================================================

        private void HomeRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ButtonBase btn)
            {
                var commandName =
                    btn.Tag as string ??
                    (btn.Content as string) ??
                    btn.Content?.ToString() ??
                    "(unknown)";

                Title = $"cb_FRAME – {commandName}";

                if (StatusModeText != null)
                {
                    StatusModeText.Text = $"Mode: {commandName}";
                }
            }
        }

        // =========================================================
        // ================ TOOL STATE / ESC HANDLING ==============
        // =========================================================

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelActiveTool();
            }
        }

        private void ActivateTool(FrameTool tool)
        {
            _activeTool = tool;
            _pendingMemberStart = null; // reset any in-progress member

            string label = GetToolDisplayName(tool);

            // Update 3D View pill
            if (ModePillText != null)
            {
                ModePillText.Text = $"Mode: {label}";
            }

            // Update status bar mode text
            if (StatusModeText != null)
            {
                StatusModeText.Text = $"Mode: {label}";
            }

            // Update window title
            Title = $"cb_FRAME – {label}";

            // Update checked state of all tool buttons
            UpdateToolButtonChecks(tool);
        }

        private void UpdateToolButtonChecks(FrameTool active)
        {
            foreach (var kvp in _toolButtons)
            {
                var isThis = (kvp.Key == active) && (active != FrameTool.None);
                kvp.Value.IsChecked = isThis;
            }
        }

        private void CancelActiveTool()
        {
            _pendingMemberStart = null;
            ActivateTool(FrameTool.None);
        }

        // ===========================
        // Quick Access toolbar stubs
        // ===========================

        private void QuickAccessNew_Click(object sender, RoutedEventArgs e)
        {
            StatusBarMessage("New model (not implemented yet)");
        }

        private void QuickAccessOpen_Click(object sender, RoutedEventArgs e)
        {
            StatusBarMessage("Open model (not implemented yet)");
        }

        private void QuickAccessSave_Click(object sender, RoutedEventArgs e)
        {
            StatusBarMessage("Save model (not implemented yet)");
        }

        private void QuickAccessUndo_Click(object sender, RoutedEventArgs e)
        {
            StatusBarMessage("Undo (not implemented yet)");
        }

        private void QuickAccessRedo_Click(object sender, RoutedEventArgs e)
        {
            StatusBarMessage("Redo (not implemented yet)");
        }

        private void StatusBarMessage(string message)
        {
            if (StatusModeText != null)
            {
                StatusModeText.Text = message;
            }
        }

        private void UpdateStatusCounts()
        {
            if (StatusCountsText != null)
            {
                var nodeLabel = _nodes.Count == 1 ? "Node" : "Nodes";
                var memberLabel = _members.Count == 1 ? "Member" : "Members";

                StatusCountsText.Text = $"{_nodes.Count} {nodeLabel}   {_members.Count} {memberLabel}";
            }
        }

        // =========================================================
        // ============= Phase 6 – Step 5: Solve wiring ============
        // =========================================================

        private void RunSolveAnalysis()
        {
            try
            {
                // ----------------------------------------------
                // 1) Call the analysis engine (still placeholder)
                // ----------------------------------------------
                int nodeCount = Math.Max(_nodes.Count, 1);

                var dofMapper = new DofMapper(
                    nodeCount,
                    _ => new[] { true, true, true });

                var elements = new List<FrameElement2D>();
                var globalForces = new double[dofMapper.FreeDofCount];

                var result = _analysisService.RunLinearStatic(
                    elements,
                    dofMapper,
                    globalForces);

                // ----------------------------------------------
                // 2) Build a FAKE deflected shape from _nodes/_members
                //    (just to prove the pipeline + drawing works)
                // ----------------------------------------------

                if (_nodes.Count == 0 || _members.Count == 0)
                {
                    MessageBox.Show(
                        "No model elements to show deflection for yet.",
                        "Analysis",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // Simple fake: push nodes down in -Y based on their X position
                // so you see a “bent” shape above the original members.
                double scale = -0.05; // tweak to taste

                var deflectedNodePositions = _nodes
                    .OrderBy(n => n.Id)
                    .Select(n =>
                        new Point3D(
                            n.Position.X,
                            n.Position.Y + n.Position.X * scale,
                            n.Position.Z))
                    .ToList();

                var deflectedMembers = _members
                    .Select(m => (StartNodeId: m.Start.Id, EndNodeId: m.End.Id))
                    .ToList();

                // exaggeration factor not used inside yet, but keep for later
                double exaggeration = 1.0;

                UpdateDeflectedShape(
                    deflectedNodePositions,
                    deflectedMembers,
                    exaggeration);

                // ----------------------------------------------
                // 3) Notify user
                // ----------------------------------------------
                MessageBox.Show(
                    "Solve tool wired successfully.\n" +
                    "Fake deflected shape has been drawn (orange overlay).",
                    "Analysis",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Analysis failed: {ex.Message}",
                    "Analysis Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // =========================================================
        // ========== 3D VIEWPORT / DRAWING (PHASE 4) ===============
        // =========================================================

        private void InitializeViewportVisuals()
        {
            if (MainViewport == null)
                return;

            _frameVisual = new ModelVisual3D();
            _deflectedShapeVisual = new ModelVisual3D();

            MainViewport.Children.Add(_frameVisual);
            MainViewport.Children.Add(_deflectedShapeVisual);
        }

        private void BuildTestModel()
        {
            _nodes.Clear();
            _members.Clear();

            var n1 = new Node3D { Id = 1, Position = new Point3D(0, 0, 0) };
            var n2 = new Node3D { Id = 2, Position = new Point3D(4, 0, 0) };
            var n3 = new Node3D { Id = 3, Position = new Point3D(0, 0, 3) };
            var n4 = new Node3D { Id = 4, Position = new Point3D(4, 0, 3) };

            _nodes.AddRange(new[] { n1, n2, n3, n4 });

            _members.Add(new Member3D { Id = 1, Start = n1, End = n3 });
            _members.Add(new Member3D { Id = 2, Start = n2, End = n4 });
            _members.Add(new Member3D { Id = 3, Start = n3, End = n4 });

            UpdateStatusCounts();
        }

        private void RenderModelToViewport()
        {
            if (_frameVisual == null)
                return;

            var group = new Model3DGroup();

            // ✅ Use HelixToolkit's WPF MeshBuilder (Point3D + double)
            var builder = new HelixToolkit.Wpf.MeshBuilder();

            const double memberRadius = 0.05;
            foreach (var m in _members)
            {
                builder.AddCylinder(m.Start.Position, m.End.Position, memberRadius, 16);
            }

            const double nodeRadius = 0.08;
            foreach (var n in _nodes)
            {
                builder.AddSphere(n.Position, nodeRadius, 12, 12);
            }

            var geometry = builder.ToMesh();  // MeshGeometry3D (WPF)
            var material = MaterialHelper.CreateMaterial(Colors.SteelBlue);
            var model = new GeometryModel3D(geometry, material)
            {
                BackMaterial = material
            };

            group.Children.Add(model);
            _frameVisual.Content = group;

            MainViewport.ZoomExtents();
            UpdateStatusCounts();
        }

        public void UpdateDeflectedShape(
                    IEnumerable<Point3D> deflectedNodePositions,
                    IEnumerable<(int StartNodeId, int EndNodeId)> deflectedMembers,
                    double exaggerationFactor)
        {
            if (_deflectedShapeVisual == null)
                return;

            var group = new Model3DGroup();

            // ✅ Use HelixToolkit's WPF MeshBuilder (Point3D + double)
            var builder = new HelixToolkit.Wpf.MeshBuilder();

            const double deflectedRadius = 0.03;

            var nodeMap = deflectedNodePositions
                .Select((p, index) => new { Id = index + 1, Position = p })
                .ToDictionary(x => x.Id, x => x.Position);

            foreach (var (startId, endId) in deflectedMembers)
            {
                if (!nodeMap.TryGetValue(startId, out var p0)) continue;
                if (!nodeMap.TryGetValue(endId, out var p1)) continue;

                builder.AddTube(new[] { p0, p1 }, deflectedRadius, 8, false);
            }

            var geometry = builder.ToMesh();  // MeshGeometry3D
            var material = MaterialHelper.CreateMaterial(Colors.OrangeRed);
            var model = new GeometryModel3D(geometry, material)
            {
                BackMaterial = material
            };

            group.Children.Add(model);
            _deflectedShapeVisual.Content = group;
        }

        // =========================
        //  VIEWPORT INPUT HANDLERS
        // =========================

        private void MainViewport_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MainViewport);

            switch (_activeTool)
            {
                case FrameTool.Nodes:
                    HandleNodeToolClick(pos);
                    break;

                case FrameTool.Members:
                    HandleMemberToolClick(pos);
                    break;

                case FrameTool.Select:
                    HandleSelectToolClick(pos);
                    break;
            }
        }

        private void HandleNodeToolClick(Point screenPos)
        {
            var worldPoint = ProjectToWorkingPlane(screenPos);
            if (worldPoint == null)
                return;

            var node = new Node3D
            {
                Id = _nodes.Any() ? _nodes.Max(n => n.Id) + 1 : 1,
                Position = worldPoint.Value
            };

            _nodes.Add(node);
            UpdateModelCounts();
            RenderModelToViewport();
            UpdateStatusCounts();
        }

        private void HandleMemberToolClick(Point screenPos)
        {
            var node = HitTestNearestNode(screenPos);
            if (node == null)
                return;

            if (_pendingMemberStart == null)
            {
                // First click – start node
                _pendingMemberStart = node;
            }
            else
            {
                if (!ReferenceEquals(_pendingMemberStart, node))
                {
                    var member = new Member3D
                    {
                        Id = _members.Any() ? _members.Max(m => m.Id) + 1 : 1,
                        Start = _pendingMemberStart,
                        End = node
                    };

                    _members.Add(member);
                    UpdateModelCounts();
                    RenderModelToViewport();
                    UpdateStatusCounts();
                }

                _pendingMemberStart = null; // done
            }
        }

        private void HandleSelectToolClick(Point screenPos)
        {
            var node = HitTestNearestNode(screenPos);
            if (node != null)
            {
                // TODO: visual highlight, property panel binding, etc.
                // StatusBarMessage($"Selected node {node.Id}");
            }
        }

        private Point3D? ProjectToWorkingPlane(Point screenPos)
        {
            if (MainViewport?.Viewport == null)
                return null;

            var planePoint = new Point3D(0, 0, 0);
            var planeNormal = new Vector3D(0, 0, 1);

            var result = Viewport3DHelper.UnProject(
                MainViewport.Viewport,
                screenPos,
                planePoint,
                planeNormal);

            return result;
        }

        /// <summary>
        /// Very simple hit test: project the click onto the working plane (Z = 0)
        /// and pick the node whose 3D position is closest to that world point.
        /// </summary>
        private Node3D? HitTestNearestNode(Point screenPos, double maxWorldDistance = 0.25)
        {
            // Convert the 2D screen point into a 3D point on Z = 0
            var worldPoint = ProjectToWorkingPlane(screenPos);
            if (worldPoint == null)
                return null;

            Node3D? bestNode = null;
            double bestDist = maxWorldDistance;

            foreach (var node in _nodes)
            {
                var dx = node.Position.X - worldPoint.Value.X;
                var dy = node.Position.Y - worldPoint.Value.Y;
                var dz = node.Position.Z - worldPoint.Value.Z;
                var dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestNode = node;
                }
            }
            return bestNode;
        }

        private void UpdateModelCounts()
        {
            int nodeCount = _nodes.Count;
            int memberCount = _members.Count;

            if (StatusCountsText != null)
            {
                StatusCountsText.Text = $"{nodeCount} Nodes   {memberCount} Members";
            }
        }
    }
}

