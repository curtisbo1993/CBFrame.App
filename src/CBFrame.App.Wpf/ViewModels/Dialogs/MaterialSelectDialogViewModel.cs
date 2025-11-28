using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using CBFrame.App.Wpf.Infrastructure;
using CBFrame.MaterialsDb.Models;

namespace CBFrame.App.Wpf.ViewModels.Dialogs
{
    /// <summary>
    /// View-model for the "Select Material" dialog.
    /// Provides a filtered view over all materials.
    /// </summary>
    public sealed class MaterialSelectDialogViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<MaterialRecord> _materials;

        public ICollectionView MaterialsView { get; }

        private MaterialRecord? _selectedMaterial;
        public MaterialRecord? SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (!Equals(_selectedMaterial, value))
                {
                    _selectedMaterial = value;
                    OnPropertyChanged(nameof(SelectedMaterial));
                    (OkCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (!string.Equals(_filterText, value, StringComparison.Ordinal))
                {
                    _filterText = value;
                    OnPropertyChanged(nameof(FilterText));
                    MaterialsView.Refresh();
                }
            }
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler? RequestCloseOk;
        public event EventHandler? RequestCloseCancel;

        public MaterialSelectDialogViewModel()
        {
            var all = AppLookupServices.Materials.GetAllMaterials();

            _materials = new ObservableCollection<MaterialRecord>(all);
            MaterialsView = CollectionViewSource.GetDefaultView(_materials);
            MaterialsView.Filter = FilterMaterial;

            OkCommand = new DelegateCommand(
                () => RequestCloseOk?.Invoke(this, EventArgs.Empty),
                () => SelectedMaterial != null);

            CancelCommand = new DelegateCommand(
                () => RequestCloseCancel?.Invoke(this, EventArgs.Empty));
        }

        private bool FilterMaterial(object obj)
        {
            if (obj is not MaterialRecord m)
                return false;

            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            var text = FilterText.Trim();

            return (m.Id?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                   || (m.Name?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                   || (m.MaterialType?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
        }

        // ------------- INotifyPropertyChanged -------------

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
