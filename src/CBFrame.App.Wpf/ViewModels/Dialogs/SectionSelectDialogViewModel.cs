using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using CBFrame.App.Wpf.Infrastructure;
using CBFrame.SectionsDb.Models;

namespace CBFrame.App.Wpf.ViewModels.Dialogs
{
    /// <summary>
    /// View-model for the "Select Section" dialog.
    /// It exposes a filtered view over all available DB sections.
    /// </summary>
    public sealed class SectionSelectDialogViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<SectionShape> _sections;

        public ICollectionView SectionsView { get; }

        private SectionShape? _selectedSection;
        public SectionShape? SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (!Equals(_selectedSection, value))
                {
                    _selectedSection = value;
                    OnPropertyChanged(nameof(SelectedSection));
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
                    SectionsView.Refresh();
                }
            }
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler? RequestCloseOk;
        public event EventHandler? RequestCloseCancel;

        public SectionSelectDialogViewModel()
        {
            // Load all sections from the lookup service
            var all = AppLookupServices.Sections.GetAllSections();

            _sections = new ObservableCollection<SectionShape>(all);
            SectionsView = CollectionViewSource.GetDefaultView(_sections);
            SectionsView.Filter = FilterSection;

            OkCommand = new DelegateCommand(
                () => RequestCloseOk?.Invoke(this, EventArgs.Empty),
                () => SelectedSection != null);

            CancelCommand = new DelegateCommand(
                () => RequestCloseCancel?.Invoke(this, EventArgs.Empty));
        }

        private bool FilterSection(object obj)
        {
            if (obj is not SectionShape s)
                return false;

            if (string.IsNullOrWhiteSpace(FilterText))
                return true;

            var text = FilterText.Trim();
            return (s.Id?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                   || (s.Name?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                   || (s.ShapeType?.IndexOf(text, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
        }

        // ---------------- INotifyPropertyChanged ----------------

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
