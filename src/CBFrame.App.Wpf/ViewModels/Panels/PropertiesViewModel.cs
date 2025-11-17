using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels.Panels;

public sealed class PropertiesViewModel : INotifyPropertyChanged
{
    public string Title { get; } = "Properties";

    public event PropertyChangedEventHandler? PropertyChanged;
}
