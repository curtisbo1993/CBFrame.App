using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

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
        private FrameTool _activeTool = FrameTool.None;

        // map each tool enum to its ToggleButton
        private readonly Dictionary<FrameTool, ToggleButton> _toolButtons =
            new Dictionary<FrameTool, ToggleButton>();

        public MainShellWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;

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

        // ========== HEADER BUTTONS ==========

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
    }
}
