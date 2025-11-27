using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CBFrame.App.Wpf.Infrastructure;
using CBFrame.Core.Loads;

namespace CBFrame.App.Wpf.ViewModels.Panels
{
    /// <summary>
    /// View-model for the Load Cases panel (Phase 7).
    /// Keeps an in-memory collection of LoadCase objects
    /// and exposes simple commands for Add / Delete / Defaults.
    /// </summary>
    public sealed class LoadCasesPanelViewModel : INotifyPropertyChanged
    {
        public string Title { get; } = "Load Cases";

        public ObservableCollection<LoadCase> LoadCases { get; }

        private LoadCase? _selected;
        public LoadCase? Selected
        {
            get => _selected;
            set
            {
                if (!Equals(_selected, value))
                {
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                    (DeleteCaseCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// List of load case types for the Type combo box.
        /// </summary>
        public Array LoadCaseTypes => Enum.GetValues(typeof(LoadCaseType));

        public ICommand AddCaseCommand { get; }
        public ICommand DeleteCaseCommand { get; }
        public ICommand GenerateDefaultsCommand { get; }

        public LoadCasesPanelViewModel()
        {
            LoadCases = new ObservableCollection<LoadCase>();

            AddCaseCommand = new DelegateCommand(AddCase);
            DeleteCaseCommand = new DelegateCommand(DeleteCase, () => Selected != null);
            GenerateDefaultsCommand = new DelegateCommand(GenerateDefaults);
        }

        private void AddCase()
        {
            int nextId = LoadCases.Count == 0
                ? 1
                : LoadCases.Max(c => c.Id) + 1;

            // Use the constructor instead of setting Id directly
            var lc = new LoadCase(nextId, $"Case {nextId}", LoadCaseType.Dead)
            {
                // these are settable properties, so object init is fine
                IncludeSelfWeight = false,
                SelfWeightFactor = 1.0,
                IsActive = true
            };

            LoadCases.Add(lc);
            Selected = lc;
        }

        private void DeleteCase()
        {
            if (Selected == null)
                return;

            LoadCases.Remove(Selected);
            Selected = null;
        }

        private void GenerateDefaults()
        {
            LoadCases.Clear();

            foreach (var lc in LoadDefaults.CreateBasicLoadCases())
            {
                LoadCases.Add(lc);
            }

            if (LoadCases.Count > 0)
                Selected = LoadCases[0];
        }

        // ===== INotifyPropertyChanged =====

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
