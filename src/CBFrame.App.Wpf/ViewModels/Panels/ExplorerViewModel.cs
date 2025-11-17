using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels.Panels;

public sealed class ExplorerViewModel : INotifyPropertyChanged
{
    public string Title { get; } = "Model Explorer";

    public event PropertyChangedEventHandler? PropertyChanged;
}
