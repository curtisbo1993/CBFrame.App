using System.Windows;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.Services;
using CBFrame.App.Wpf.Tools;
using CBFrame.App.Wpf.ViewModels;

namespace CBFrame.App.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ===== Core model =====
            var document = new FrameDocument();

            // ===== Services =====
            var selectionService = new SelectionService();
            var undoRedoService = new UndoRedoService();
            var modelEditingService = new ModelEditingService(undoRedoService, document);

            // ===== Tools =====
            var selectTool = new SelectTool(selectionService, document);
            var drawNodeTool = new DrawNodeTool(modelEditingService);
            var drawMemberTool = new DrawMemberTool(modelEditingService);

            var toolController = new ToolController(selectTool, drawNodeTool, drawMemberTool);

            // ===== Main ViewModel =====
            DataContext = new MainViewModel(selectionService, toolController, undoRedoService, document);
        }
    }
}
