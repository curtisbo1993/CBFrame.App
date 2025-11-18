using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels.Panels
{
    public sealed class ExplorerViewModel : INotifyPropertyChanged
    {
        // Simple title (you already had this conceptually)
        public string Title { get; } = "Explorer";

        // The object currently selected in the app (node, member, etc.)
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            private set
            {
                if (!ReferenceEquals(_selectedObject, value))
                {
                    _selectedObject = value;
                    OnPropertyChanged(nameof(SelectedObject));
                }
            }
        }

        // Called by MainViewModel when selection changes
        public void UpdateSelection(object? selected)
        {
            SelectedObject = selected;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
