using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels.Panels;

public sealed class Model3DViewModel : INotifyPropertyChanged
{
    public string Title { get; } = "3D Viewport";

    public event PropertyChangedEventHandler? PropertyChanged;
}
