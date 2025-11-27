using CBFrame.Analysis.LinearStatic;
using CBFrame.App.Wpf.Services;
using CBFrame.Core.Geometry;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Views;
using CBFrame.App.Wpf.ViewModels.Panels;

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

    // ==========================================================
    // CUSTOM MESHBUILDER FOR cb_FRAME  (no Helix MeshBuilder)
    // ==========================================================
    public class MeshBuilder
    {
        private readonly List<Point3D> _positions = new();
        private readonly List<int> _triangles = new();

        public void AddCylinder(Point3D p0, Point3D p1, double radius, int thetaDiv)
        {
            if (thetaDiv < 3) thetaDiv = 3;

            Vector3D axis = p1 - p0;
            double length = axis.Length;
            if (length <= 1e-6) return;

            axis.Normalize();

            // Build an orthonormal basis (u, v, axis)
            Vector3D arbitrary = Math.Abs(axis.X) < 0.9
                ? new Vector3D(1, 0, 0)
                : new Vector3D(0, 1, 0);

            Vector3D u = Vector3D.CrossProduct(axis, arbitrary);
            if (u.LengthSquared < 1e-6)
            {
                arbitrary = new Vector3D(0, 0, 1);
                u = Vector3D.CrossProduct(axis, arbitrary);
            }
            u.Normalize();

            Vector3D v = Vector3D.CrossProduct(axis, u);
            v.Normalize();

            int baseIndex = _positions.Count;

            // Ring of vertices around p0 and p1
            for (int i = 0; i <= thetaDiv; i++)
            {
                double angle = 2.0 * Math.PI * i / thetaDiv;
                double cos = Math.Cos(angle);
                double sin = Math.Sin(angle);
                Vector3D offset = (u * cos + v * sin) * radius;

                _positions.Add(p0 + offset); // bottom
                _positions.Add(p1 + offset); // top
            }

            // Side faces
            for (int i = 0; i < thetaDiv; i++)
            {
                int i0 = baseIndex + i * 2;
                int i1 = baseIndex + i * 2 + 1;
                int i2 = baseIndex + (i + 1) * 2;
                int i3 = baseIndex + (i + 1) * 2 + 1;

                AddTriangle(i0, i1, i3);
                AddTriangle(i0, i3, i2);
            }
        }

        // Nodes: we just draw little boxes (good enough visually)
        public void AddSphere(Point3D center, double radius, int thetaDiv, int phiDiv)
        {
            AddBox(center, radius, radius, radius);
        }

        // Deflected shape: tube along a polyline
        public void AddTube(IEnumerable<Point3D> path, double radius, int thetaDiv, bool isClosed)
        {
            if (path == null) return;

            var pts = path.ToList();
            if (pts.Count < 2) return;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                AddCylinder(pts[i], pts[i + 1], radius, thetaDiv);
            }

            if (isClosed && pts.Count > 2)
            {
                AddCylinder(pts[^1], pts[0], radius, thetaDiv);
            }
        }

        public MeshGeometry3D ToMesh()
        {
            return new MeshGeometry3D
            {
                Positions = new Point3DCollection(_positions),
                TriangleIndices = new Int32Collection(_triangles)
            };
        }

        // ---------- helpers ----------

        private void AddTriangle(int i0, int i1, int i2)
        {
            _triangles.Add(i0);
            _triangles.Add(i1);
            _triangles.Add(i2);
        }

        private void AddBox(Point3D center, double hx, double hy, double hz)
        {
            int baseIndex = _positions.Count;

            var p000 = new Point3D(center.X - hx, center.Y - hy, center.Z - hz);
            var p001 = new Point3D(center.X - hx, center.Y - hy, center.Z + hz);
            var p010 = new Point3D(center.X - hx, center.Y + hy, center.Z - hz);
            var p011 = new Point3D(center.X - hx, center.Y + hy, center.Z + hz);
            var p100 = new Point3D(center.X + hx, center.Y - hy, center.Z - hz);
            var p101 = new Point3D(center.X + hx, center.Y - hy, center.Z + hz);
            var p110 = new Point3D(center.X + hx, center.Y + hy, center.Z - hz);
            var p111 = new Point3D(center.X + hx, center.Y + hy, center.Z + hz);

            _positions.Add(p000); // 0
            _positions.Add(p001); // 1
            _positions.Add(p010); // 2
            _positions.Add(p011); // 3
            _positions.Add(p100); // 4
            _positions.Add(p101); // 5
            _positions.Add(p110); // 6
            _positions.Add(p111); // 7

            void T(int a, int b, int c) => AddTriangle(baseIndex + a, baseIndex + b, baseIndex + c);

            // front  (z+)
            T(1, 5, 7); T(1, 7, 3);
            // back   (z-)
            T(0, 2, 6); T(0, 6, 4);
            // left   (x-)
            T(0, 1, 3); T(0, 3, 2);
            // right  (x+)
            T(4, 6, 7); T(4, 7, 5);
            // top    (y+)
            T(2, 3, 7); T(2, 7, 6);
            // bottom (y-)
            T(0, 4, 5); T(0, 5, 1);
        }
    }

    public partial class MainShellWindow : Window
    {

        // Phase 7 – load panels
        private readonly LoadCasesPanelView _loadCasesPanelView;
        private readonly LoadCasesPanelViewModel _loadCasesPanelViewModel;
        private readonly LoadCombinationsPanelView _loadCombinationsPanelView;
        private readonly LoadCombinationsPanelViewModel _loadCombinationsPanelViewModel;

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

        // =======================================================
        // FAKE DEFLECTED SHAPE — Phase 6 temporary visualization
        // =======================================================

        private List<Point3D> GetFakeDeflectedPoints(Member3D member)
        {
            double dx = member.End.Position.X - member.Start.Position.X;
            double dy = member.End.Position.Y - member.Start.Position.Y;
            double dz = member.End.Position.Z - member.Start.Position.Z;

            double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double scale = length * 0.05; // 5% deflection

            var p0 = member.Start.Position;
            var p1 = new Point3D(
                member.End.Position.X,
                member.End.Position.Y,
                member.End.Position.Z + scale
            );

            return new List<Point3D> { p0, p1 };
        }

        private void RenderFakeDeflectedShape()
        {
            if (_deflectedShapeVisual == null)
                return;

            var group = new Model3DGroup();

            foreach (var m in _members)
            {
                var pts = GetFakeDeflectedPoints(m);

                var builder = new MeshBuilder();
                builder.AddTube(
                    pts,
                    radius: 0.05,
                    thetaDiv: 12,
                    isClosed: false
                );

                var mesh = builder.ToMesh();
                var material = Materials.Yellow;

                group.Children.Add(new GeometryModel3D(mesh, material));
            }

            _deflectedShapeVisual.Content = group;
        }

        // Viewport visuals
        private ModelVisual3D? _frameVisual;           // undeformed frame (nodes + members)
        private ModelVisual3D? _deflectedShapeVisual;  // deflected shapes

        // Member drawing state (for FrameTool.Members)
        private Node3D? _pendingMemberStart;
        // Selection state
        private Node3D? _selectedNode;
        private Member3D? _selectedMember;

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
            _toolButtons[FrameTool.Select] = ToolSelect;

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

            // ========== Phase 7: Load Cases panel ==========
            _loadCasesPanelViewModel = new LoadCasesPanelViewModel();
            _loadCasesPanelView = new LoadCasesPanelView
            {
                DataContext = _loadCasesPanelViewModel
            };

            // ========== Phase 7: Load Combinations panel ==========
            _loadCombinationsPanelViewModel = new LoadCombinationsPanelViewModel(_loadCasesPanelViewModel.LoadCases);
            _loadCombinationsPanelView = new LoadCombinationsPanelView
            {
                DataContext = _loadCombinationsPanelViewModel
            };

            if (DataPanelHost != null)
            {
                // Start hidden – we show the appropriate panel based on the active tool
                DataPanelHost.Content = null;
                DataPanelHost.Visibility = Visibility.Collapsed;
            }
        }   // <-- closes MainShellWindow constructor

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
            // Select
            if (btn == ToolSelect) return FrameTool.Select;

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
                return;
            }

            // Delete key = delete current selection (member first, then node)
            if (e.Key == Key.Delete)
            {
                bool deleted = false;

                if (_selectedMember != null)
                {
                    _members.Remove(_selectedMember);
                    _selectedMember = null;
                    deleted = true;
                }
                else if (_selectedNode != null)
                {
                    _members.RemoveAll(m => m.Start == _selectedNode || m.End == _selectedNode);
                    _nodes.Remove(_selectedNode);
                    _selectedNode = null;
                    deleted = true;
                }

                if (deleted)
                {
                    RenderModelToViewport();
                    UpdateStatusCounts();
                    StatusBarMessage("Deleted selection.");
                }
            }
        }

        private void ActivateTool(FrameTool tool)
        {
            _activeTool = tool;
            _pendingMemberStart = null; // reset any in-progress member

            string label = GetToolDisplayName(tool);

            if (ModePillText != null)
                ModePillText.Text = $"Mode: {label}";

            if (StatusModeText != null)
                StatusModeText.Text = $"Mode: {label}";

            Title = $"cb_FRAME – {label}";

            UpdateToolButtonChecks(tool);

            // Phase 7 – show the appropriate load panel
            if (DataPanelHost != null)
            {
                if (tool == FrameTool.BasicLoadCases)
                {
                    DataPanelHost.Content = _loadCasesPanelView;
                    DataPanelHost.Visibility = Visibility.Visible;
                }
                else if (tool == FrameTool.LoadCombinations)
                {
                    DataPanelHost.Content = _loadCombinationsPanelView;
                    DataPanelHost.Visibility = Visibility.Visible;
                }
                else
                {
                    DataPanelHost.Content = null;
                    DataPanelHost.Visibility = Visibility.Collapsed;
                }
            }
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

        // =========================
        // Quick Access toolbar stubs
        // =========================

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
            StatusBarMessage("Running analysis (demo)…");

            // OLD temporary visuals – turn them OFF for now
            // RenderFakeDeflectedShape();

            try
            {
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

                // Also comment this line out for now:
                // UpdateDeflectedShape(deflectedNodePositions, deflectedMembers, exaggeration);

                if (StatusSolutionText != null)
                    StatusSolutionText.Text = "Linear Static (demo)";

                StatusBarMessage("Analysis finished (demo solve).");

                MessageBox.Show(
                    "Solve tool wired successfully.\n" +
                    "Real deflected shapes will be shown once the analysis engine is fully wired.",
                    "Analysis",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                if (_nodes.Count == 0 || _members.Count == 0)
                {
                    MessageBox.Show(
                        "No model elements to show deflection for yet.",
                        "Analysis",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    if (StatusSolutionText != null)
                        StatusSolutionText.Text = "Linear Static (no elements)";

                    return;
                }

                double scale = -0.05; // fake vertical deflection scale

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

                double exaggeration = 2.5;

                UpdateDeflectedShape(
                    deflectedNodePositions,
                    deflectedMembers,
                    exaggeration);

                if (StatusSolutionText != null)
                {
                    StatusSolutionText.Text = "Linear Static (demo)";
                }

                StatusBarMessage("Analysis finished (demo solve).");

                MessageBox.Show(
                    "Solve tool wired successfully.\n" +
                    "Fake deflected shape has been drawn (orange overlay).",
                    "Analysis",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                if (StatusSolutionText != null)
                {
                    StatusSolutionText.Text = "Error";
                }

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

            MainViewport.Children.Clear();

            _frameVisual = new ModelVisual3D();
            _deflectedShapeVisual = new ModelVisual3D();

            MainViewport.Children.Add(_deflectedShapeVisual);  // draw behind
            MainViewport.Children.Add(_frameVisual);           // draw on top

        }

        private void BuildTestModel()
        {
            _nodes.Clear();
            _members.Clear();

            const double liftY = 1.0;

            var n1 = new Node3D { Id = 1, Position = new Point3D(0, liftY, 0) };
            var n2 = new Node3D { Id = 2, Position = new Point3D(4, liftY, 0) };
            var n3 = new Node3D { Id = 3, Position = new Point3D(0, liftY, 3) };
            var n4 = new Node3D { Id = 4, Position = new Point3D(4, liftY, 3) };

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

            // === Members: normal + selected ===
            var memberBuilder = new MeshBuilder();
            var memberSelectedBuilder = new MeshBuilder();

            const double memberRadius = 0.12;

            foreach (var m in _members)
            {
                bool isSelected = (m == _selectedMember);
                var b = isSelected ? memberSelectedBuilder : memberBuilder;
                b.AddCylinder(m.Start.Position, m.End.Position, memberRadius, 16);
            }

            // Normal members (cyan)
            var memberGeom = memberBuilder.ToMesh();
            if (memberGeom != null && memberGeom.Positions != null && memberGeom.Positions.Count > 0)
            {
                var memberMat = MaterialHelper.CreateMaterial(Color.FromRgb(0, 220, 255)); // cyan
                group.Children.Add(new GeometryModel3D(memberGeom, memberMat) { BackMaterial = memberMat });
            }

            // Selected members (yellow)
            var memberSelGeom = memberSelectedBuilder.ToMesh();
            if (memberSelGeom != null && memberSelGeom.Positions != null && memberSelGeom.Positions.Count > 0)
            {
                var memberSelMat = MaterialHelper.CreateMaterial(Colors.Yellow);
                group.Children.Add(new GeometryModel3D(memberSelGeom, memberSelMat) { BackMaterial = memberSelMat });
            }

            // === Nodes: normal + selected ===
            var nodeBuilder = new MeshBuilder();
            var nodeSelectedBuilder = new MeshBuilder();

            const double nodeRadius = 0.15;

            foreach (var n in _nodes)
            {
                bool isSelected = (n == _selectedNode);
                var b = isSelected ? nodeSelectedBuilder : nodeBuilder;
                b.AddSphere(n.Position, nodeRadius, 12, 12);
            }

            // Normal nodes (cyan)
            var nodeGeom = nodeBuilder.ToMesh();
            if (nodeGeom != null && nodeGeom.Positions != null && nodeGeom.Positions.Count > 0)
            {
                var nodeMat = MaterialHelper.CreateMaterial(Color.FromRgb(0, 220, 255));
                group.Children.Add(new GeometryModel3D(nodeGeom, nodeMat) { BackMaterial = nodeMat });
            }

            // Selected nodes (yellow)
            var nodeSelGeom = nodeSelectedBuilder.ToMesh();
            if (nodeSelGeom != null && nodeSelGeom.Positions != null && nodeSelGeom.Positions.Count > 0)
            {
                var nodeSelMat = MaterialHelper.CreateMaterial(Colors.Yellow);
                group.Children.Add(new GeometryModel3D(nodeSelGeom, nodeSelMat) { BackMaterial = nodeSelMat });
            }

            _frameVisual.Content = group;

            MainViewport.ZoomExtents();
            UpdateStatusCounts();
            Dispatcher.InvokeAsync(() => MainViewport.ZoomExtents());
        }

        public void UpdateDeflectedShape(
            IEnumerable<Point3D> deflectedNodePositions,
            IEnumerable<(int StartNodeId, int EndNodeId)> deflectedMembers,
            double exaggerationFactor)
        {
            if (_deflectedShapeVisual == null)
                return;

            var group = new Model3DGroup();
            var builder = new MeshBuilder();

            // VERY THICK
            const double deflectedRadius = 0.15;

            // BRIGHT color (impossible to miss)
            var overlayColor = Color.FromRgb(255, 0, 255); // MAGENTA
            var material = MaterialHelper.CreateMaterial(overlayColor);

            // Push the deflected shape forward in +Z so it floats in front
            var nodeMap = deflectedNodePositions
                .Select((p, index) => new
                {
                    Id = index + 1,
                    Position = new Point3D(
                        p.X,
                        p.Y,
                        p.Z + 2.0 // big offset: FLOAT THE SHAPE FORWARD
                    )
                })
                .ToDictionary(x => x.Id, x => x.Position);

            foreach (var (startId, endId) in deflectedMembers)
            {
                if (!nodeMap.TryGetValue(startId, out var p0)) continue;
                if (!nodeMap.TryGetValue(endId, out var p1)) continue;

                builder.AddTube(new[] { p0, p1 }, deflectedRadius, 16, false);
            }

            var geom = builder.ToMesh();
            var model = new GeometryModel3D(geom, material)
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

                _pendingMemberStart = null;
            }
        }

        private void HandleSelectToolClick(Point screenPos)
        {
            var node = HitTestNearestNode(screenPos);
            if (node != null)
            {
                _selectedNode = node;
                _selectedMember = null;
                StatusBarMessage($"Selected node {node.Id}");
                RenderModelToViewport();
                return;
            }

            var member = HitTestNearestMember(screenPos);
            if (member != null)
            {
                _selectedMember = member;
                _selectedNode = null;
                StatusBarMessage($"Selected member {member.Id}");
                RenderModelToViewport();
                return;
            }

            if (_selectedNode != null || _selectedMember != null)
            {
                _selectedNode = null;
                _selectedMember = null;
                StatusBarMessage("Selection cleared");
                RenderModelToViewport();
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

        private Node3D? HitTestNearestNode(Point screenPos, double maxWorldDistance = 0.25)
        {
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

        private Member3D? HitTestNearestMember(Point screenPos, double maxWorldDistance = 0.25)
        {
            var worldPoint = ProjectToWorkingPlane(screenPos);
            if (worldPoint == null)
                return null;

            Member3D? bestMember = null;
            double bestDist = maxWorldDistance;

            foreach (var member in _members)
            {
                var p0 = member.Start.Position;
                var p1 = member.End.Position;

                var vx = p1.X - p0.X;
                var vy = p1.Y - p0.Y;
                var vz = p1.Z - p0.Z;

                var wx = worldPoint.Value.X - p0.X;
                var wy = worldPoint.Value.Y - p0.Y;
                var wz = worldPoint.Value.Z - p0.Z;

                double segLen2 = vx * vx + vy * vy + vz * vz;
                if (segLen2 < 1e-6)
                    continue;

                double t = (vx * wx + vy * wy + vz * wz) / segLen2;
                t = Math.Max(0.0, Math.Min(1.0, t));

                var cx = p0.X + t * vx;
                var cy = p0.Y + t * vy;
                var cz = p0.Z + t * vz;

                var dx = worldPoint.Value.X - cx;
                var dy = worldPoint.Value.Y - cy;
                var dz = worldPoint.Value.Z - cz;
                double dist = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestMember = member;
                }
            }

            return bestMember;
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
