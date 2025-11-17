using System.ComponentModel;
using CBFrame.App.Wpf.ViewModels.Panels;

namespace CBFrame.App.Wpf.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        // ===== Panels =====
        public ExplorerViewModel Explorer { get; }
        public Model3DViewModel Model3D { get; }
        public PropertiesViewModel Properties { get; }
        public DataEntryViewModel DataEntry { get; }

        // ===== Status Bar =====
        public string StatusText { get; private set; } = "Ready";
        public string RightStatusText { get; private set; } = "cb_FRAME • Preview Shell";

        public MainViewModel()
        {
            // Initialize panel view-models with simple titles (you already set these)
            Explorer = new ExplorerViewModel();
            Model3D = new Model3DViewModel();
            Properties = new PropertiesViewModel();
            DataEntry = new DataEntryViewModel();
        }

        // ===== INotifyPropertyChanged plumbing (for future updates) =====
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper methods for later (we don’t *have* to use them yet)
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
