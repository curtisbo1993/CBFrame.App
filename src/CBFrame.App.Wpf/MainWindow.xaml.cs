using System.IO;
using System.Windows;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.Services;
using CBFrame.App.Wpf.Tools;
using CBFrame.App.Wpf.ViewModels;

namespace CBFrame.App.Wpf
{
    public partial class MainWindow : Window
    {
        // ===== Fields for Phase 5 =====
        private readonly IProjectFileService _projectFileService;
        private readonly IDialogService _dialogService;

        private readonly FrameDocument _document;
        private readonly SelectionService _selectionService;
        private readonly UndoRedoService _undoRedoService;
        private readonly ModelEditingService _modelEditingService;

        private string? _currentFilePath;

        public MainWindow()
        {
            InitializeComponent();

            // ===== Core model =====
            _document = new FrameDocument();

            // ===== Services =====
            _selectionService = new SelectionService();
            _undoRedoService = new UndoRedoService();
            _modelEditingService = new ModelEditingService(_undoRedoService, _document);

            // ===== Tools =====
            var selectTool = new SelectTool(_selectionService, _document);
            var drawNodeTool = new DrawNodeTool(_modelEditingService);
            var drawMemberTool = new DrawMemberTool(_modelEditingService);

            var toolController = new ToolController(selectTool, drawNodeTool, drawMemberTool);

            // ===== Main ViewModel =====
            DataContext = new MainViewModel(_selectionService, toolController, _undoRedoService, _document);

            // ===== File / dialog services =====
            _projectFileService = new ProjectFileService();
            _dialogService = new DialogService();

            // Start window title
            Title = "cb_FRAME – Structural Analysis";
        }

        // =====================================================================
        //  New / Open / Save handlers  (Step 4.2)
        // =====================================================================

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // Clear the existing in-memory document.
            _document.Nodes.Clear();
            _document.Members.Clear();

            _currentFilePath = null;
            Title = "cb_FRAME – Structural Analysis";
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            var path = _dialogService.ShowOpenProjectDialog();
            if (string.IsNullOrWhiteSpace(path))
                return; // user cancelled

            try
            {
                // Load into a temporary document using the file service
                var loadedDoc = _projectFileService.LoadFromFile(path);

                // Copy nodes/members into the existing document instance
                _document.Nodes.Clear();
                _document.Members.Clear();

                foreach (var node in loadedDoc.Nodes)
                    _document.Nodes.Add(node);

                foreach (var member in loadedDoc.Members)
                    _document.Members.Add(member);

                _currentFilePath = path;
                Title = $"cb_FRAME – {Path.GetFileName(path)}";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Failed to open project:\n{ex.Message}",
                    "Open Project",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            // If we don't have a file yet, do a "Save As"
            if (string.IsNullOrWhiteSpace(_currentFilePath))
            {
                var savePath = _dialogService.ShowSaveProjectDialog("Untitled");
                if (string.IsNullOrWhiteSpace(savePath))
                    return; // user cancelled

                _currentFilePath = savePath;
            }

            try
            {
                _projectFileService.SaveToFile(_currentFilePath!, _document);
                Title = $"cb_FRAME – {Path.GetFileName(_currentFilePath)}";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Failed to save project:\n{ex.Message}",
                    "Save Project",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
