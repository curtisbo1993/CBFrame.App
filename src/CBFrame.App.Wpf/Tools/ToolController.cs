using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CBFrame.App.Wpf.Tools
{
    public class ToolController : INotifyPropertyChanged
    {
        public ITool SelectTool { get; }
        public ITool DrawNodeTool { get; }
        public ITool DrawMemberTool { get; }

        private ITool _activeTool;

        public ITool ActiveTool
        {
            get => _activeTool;
            private set
            {
                if (_activeTool == value)
                    return;

                _activeTool?.OnCancel();

                _activeTool = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDrawingToolActive));
                ActiveToolChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsDrawingToolActive => ActiveTool != SelectTool;

        public ToolController(ITool selectTool, ITool drawNodeTool, ITool drawMemberTool)
        {
            SelectTool = selectTool ?? throw new ArgumentNullException(nameof(selectTool));
            DrawNodeTool = drawNodeTool ?? throw new ArgumentNullException(nameof(drawNodeTool));
            DrawMemberTool = drawMemberTool ?? throw new ArgumentNullException(nameof(drawMemberTool));

            _activeTool = SelectTool;
        }

        public void UseSelectTool() => ActiveTool = SelectTool;
        public void UseDrawNodeTool() => ActiveTool = DrawNodeTool;
        public void UseDrawMemberTool() => ActiveTool = DrawMemberTool;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ActiveToolChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
