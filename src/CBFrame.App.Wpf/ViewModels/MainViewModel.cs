using System;
using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _windowTitle = "cb_FRAME – Structural Analysis";
        private string _statusText = "Ready";
        private string _rightStatusText = DateTime.Now.ToShortTimeString();

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                if (_windowTitle != value)
                {
                    _windowTitle = value;
                    OnPropertyChanged(nameof(WindowTitle));
                }
            }
        }

        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public string RightStatusText
        {
            get => _rightStatusText;
            set
            {
                if (_rightStatusText != value)
                {
                    _rightStatusText = value;
                    OnPropertyChanged(nameof(RightStatusText));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
