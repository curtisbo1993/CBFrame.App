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
    /// View-model for the Load Combinations panel (Phase 7).
    /// </summary>
    public sealed class LoadCombinationsPanelViewModel : INotifyPropertyChanged
    {
        public string Title { get; } = "Load Combinations";

        /// <summary>
        /// All load cases available in the model.
        /// We use this when generating default combinations.
        /// </summary>
        private readonly ObservableCollection<LoadCase> _loadCases;

        public ObservableCollection<LoadCombination> Combinations { get; }

        private LoadCombination? _selected;
        public LoadCombination? Selected
        {
            get => _selected;
            set
            {
                if (!Equals(_selected, value))
                {
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                    (DeleteComboCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// For the Type combo box.
        /// </summary>
        public Array CombinationTypes => Enum.GetValues(typeof(LoadCombinationType));

        public ICommand AddComboCommand { get; }
        public ICommand DeleteComboCommand { get; }
        public ICommand GenerateDefaultsCommand { get; }

        public LoadCombinationsPanelViewModel(ObservableCollection<LoadCase> loadCases)
        {
            _loadCases = loadCases ?? throw new ArgumentNullException(nameof(loadCases));
            Combinations = new ObservableCollection<LoadCombination>();

            AddComboCommand = new DelegateCommand(AddCombination);
            DeleteComboCommand = new DelegateCommand(DeleteCombination, () => Selected != null);
            GenerateDefaultsCommand = new DelegateCommand(GenerateDefaults);
        }

        private void AddCombination()
        {
            // Next Id based on existing combinations
            int nextId = Combinations.Count == 0
                ? 1
                : Combinations.Max(c => c.Id) + 1;

            // Pick a safe default type (first enum value) so we don't depend
            // on a specific name like Strength/Service/etc.
            var defaultType = (LoadCombinationType)Enum
                .GetValues(typeof(LoadCombinationType))
                .GetValue(0)!;

            var combo = new LoadCombination(nextId, $"Combo {nextId}", defaultType)
            {
                IsActive = true
            };

            Combinations.Add(combo);
            Selected = combo;
        }

        private void DeleteCombination()
        {
            if (Selected == null)
                return;

            Combinations.Remove(Selected);
            Selected = null;
        }

        private void GenerateDefaults()
        {
            Combinations.Clear();

            // If you have no load cases yet, nothing to generate
            if (_loadCases.Count == 0)
                return;

            var defaultType = (LoadCombinationType)Enum
                .GetValues(typeof(LoadCombinationType))
                .GetValue(0)!;

            int id = 1;

            // Very simple starter combinations – you can refine the actual
            // load factors later when we wire in the real analysis engine.
            var combo1 = new LoadCombination(id++, "1.0 DL", defaultType)
            {
                IsActive = true
            };

            var combo2 = new LoadCombination(id++, "1.0 DL + 1.0 LL", defaultType)
            {
                IsActive = true
            };

            Combinations.Add(combo1);
            Combinations.Add(combo2);

            Selected = Combinations.FirstOrDefault();
        }

        // ===== INotifyPropertyChanged =====

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
