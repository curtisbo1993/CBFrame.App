using System.ComponentModel;

namespace CBFrame.App.Wpf.ViewModels.Panels;

public sealed class DataEntryViewModel : INotifyPropertyChanged
{
    public string Title { get; } = "Data Entry";

    public event PropertyChangedEventHandler? PropertyChanged;
}
