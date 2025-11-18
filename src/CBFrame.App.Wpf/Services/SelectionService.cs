using System;

namespace CBFrame.App.Wpf.Services
{
    /// <summary>
    /// Central selection service shared by viewport, explorer, and properties.
    /// </summary>
    public class SelectionService : ISelectionService
    {
        private object? _selectedObject;

        /// <summary>
        /// Currently-selected object, or null if nothing is selected.
        /// </summary>
        public object? SelectedObject => _selectedObject;

        /// <summary>
        /// Raised whenever SelectedObject changes.
        /// </summary>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Sets the current selection. Pass null to clear.
        /// </summary>
        public void SetSelection(object? selected)
        {
            // Avoid firing the event when nothing actually changed
            if (ReferenceEquals(_selectedObject, selected))
                return;

            _selectedObject = selected;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            SetSelection(null);
        }
    }
}
