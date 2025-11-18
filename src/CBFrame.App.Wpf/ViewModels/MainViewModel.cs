using System;
using System.ComponentModel;
using System.Windows.Input;
using CBFrame.App.Wpf.Infrastructure;
using CBFrame.App.Wpf.Model;
using CBFrame.App.Wpf.Services;
using CBFrame.App.Wpf.Tools;
using CBFrame.App.Wpf.ViewModels.Panels;

namespace CBFrame.App.Wpf.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        // ===== Services =====
        public ISelectionService SelectionService { get; }
        public IUndoRedoService UndoRedoService { get; }
        public FrameDocument Document { get; }

        // ===== Tools =====
        public ToolController ToolController { get; }

        // ===== Commands (for Ribbon / Buttons) =====
        public ICommand SelectToolCommand { get; }
        public ICommand DrawNodeToolCommand { get; }
        public ICommand DrawMemberToolCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        // ===== Convenience properties =====
        public string ActiveToolName => ToolController.ActiveTool.Name;
        public object? SelectedObject => SelectionService.SelectedObject;
        public bool CanUndo => UndoRedoService.CanUndo;
        public bool CanRedo => UndoRedoService.CanRedo;

        // ===== Panels =====
        public ExplorerViewModel Explorer { get; }
        public Model3DViewModel Model3D { get; }
        public PropertiesViewModel Properties { get; }
        public DataEntryViewModel DataEntry { get; }

        // ===== Status Bar =====
        public string StatusText { get; private set; } = "Ready";
        public string RightStatusText { get; private set; } = "cb_FRAME • Preview Shell";

        // ===== Constructor =====
        public MainViewModel(
            ISelectionService selectionService,
            ToolController toolController,
            IUndoRedoService undoRedoService,
            FrameDocument document)
        {
            SelectionService = selectionService ?? throw new ArgumentNullException(nameof(selectionService));
            ToolController = toolController ?? throw new ArgumentNullException(nameof(toolController));
            UndoRedoService = undoRedoService ?? throw new ArgumentNullException(nameof(undoRedoService));
            Document = document ?? throw new ArgumentNullException(nameof(document));

            // ----- Commands -----
            SelectToolCommand = new DelegateCommand(
                () => ToolController.UseSelectTool());

            DrawNodeToolCommand = new DelegateCommand(
                () => ToolController.UseDrawNodeTool());

            DrawMemberToolCommand = new DelegateCommand(
                () => ToolController.UseDrawMemberTool());

            UndoCommand = new DelegateCommand(
                () => UndoRedoService.Undo(),
                () => UndoRedoService.CanUndo);

            RedoCommand = new DelegateCommand(
                () => UndoRedoService.Redo(),
                () => UndoRedoService.CanRedo);

            // ----- Event subscriptions -----

            // React when the selection changes
            SelectionService.SelectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(SelectedObject));

                // Push selection into panels
                Explorer.UpdateSelection(SelectionService.SelectedObject);
                Properties.UpdateSelection(SelectionService.SelectedObject);
            };

            // React when the active tool changes
            ToolController.ActiveToolChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(ActiveToolName));
            };

            // React when undo/redo state changes
            UndoRedoService.StateChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));

                (UndoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (RedoCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            };

            // ----- Initialize panel VMs -----
            Explorer = new ExplorerViewModel();
            Model3D = new Model3DViewModel(toolController, document);
            Properties = new PropertiesViewModel();
            DataEntry = new DataEntryViewModel();
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ===== Status Bar Helpers =====
        public void SetStatus(string status)
        {
            StatusText = status;
            OnPropertyChanged(nameof(StatusText));
        }

        public void SetRightStatus(string status)
        {
            RightStatusText = status;
            OnPropertyChanged(nameof(RightStatusText));
        }
    }
}
