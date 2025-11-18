using System;

namespace CBFrame.App.Wpf.Services
{
    public interface ISelectionService
    {
        object? SelectedObject { get; }

        event EventHandler? SelectionChanged;

        void SetSelection(object? selected);
        void ClearSelection();
    }
}
