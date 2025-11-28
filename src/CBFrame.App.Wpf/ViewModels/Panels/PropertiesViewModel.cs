using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using CBFrame.App.Wpf.Infrastructure;
using CBFrame.App.Wpf.Model;

namespace CBFrame.App.Wpf.ViewModels.Panels
{
    /// <summary>
    /// View-model for the Properties panel.
    /// For Phase 8 we focus on exposing member section / material selection.
    /// </summary>
    public sealed class PropertiesViewModel : INotifyPropertyChanged
    {
        public string Title { get; } = "Properties";

        private object? _selectedObject;

        /// <summary>
        /// Raw selected object coming from the selection service.
        /// </summary>
        public object? SelectedObject
        {
            get => _selectedObject;
            private set
            {
                if (!ReferenceEquals(_selectedObject, value))
                {
                    _selectedObject = value;
                    OnPropertyChanged(nameof(SelectedObject));
                    OnPropertyChanged(nameof(SelectedMember));
                    OnPropertyChanged(nameof(MemberSectionDisplay));
                    OnPropertyChanged(nameof(MemberMaterialDisplay));

                    (SelectMemberSectionCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                    (SelectMemberMaterialCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Strongly-typed view over the selection when a MemberModel is selected.
        /// </summary>
        public MemberModel? SelectedMember => SelectedObject as MemberModel;

        // ===== Commands for sections / materials =====

        public ICommand SelectMemberSectionCommand { get; }
        public ICommand SelectMemberMaterialCommand { get; }

        public PropertiesViewModel()
        {
            SelectMemberSectionCommand = new DelegateCommand(
                ExecuteSelectMemberSection,
                () => SelectedMember != null);

            SelectMemberMaterialCommand = new DelegateCommand(
                ExecuteSelectMemberMaterial,
                () => SelectedMember != null);
        }

        private void ExecuteSelectMemberSection()
        {
            if (SelectedMember == null)
                return;

            var dialog = new Views.Dialogs.SectionSelectDialog
            {
                Owner = Application.Current.MainWindow
            };

            var result = dialog.ShowDialog();
            if (result == true && dialog.SelectedSection != null)
            {
                var section = dialog.SelectedSection;
                SelectedMember.SectionId = section.Id;

                OnPropertyChanged(nameof(MemberSectionDisplay));
            }
        }

        private void ExecuteSelectMemberMaterial()
        {
            if (SelectedMember == null)
                return;

            var dialog = new Views.Dialogs.MaterialSelectDialog
            {
                Owner = Application.Current.MainWindow
            };

            var result = dialog.ShowDialog();
            if (result == true && dialog.SelectedMaterial != null)
            {
                var material = dialog.SelectedMaterial;
                SelectedMember.MaterialId = material.Id;

                OnPropertyChanged(nameof(MemberMaterialDisplay));
            }
        }

        /// <summary>
        /// Friendly text for the currently selected member's section.
        /// </summary>
        public string MemberSectionDisplay
        {
            get
            {
                var member = SelectedMember;
                if (member?.SectionId is not { Length: > 0 } id)
                    return "(no section)";

                var sec = AppLookupServices.Sections.FindById(id);
                return sec != null ? $"{sec.Name} ({sec.Id})" : id;
            }
        }

        /// <summary>
        /// Friendly text for the currently selected member's material.
        /// </summary>
        public string MemberMaterialDisplay
        {
            get
            {
                var member = SelectedMember;
                if (member?.MaterialId is not { Length: > 0 } id)
                    return "(no material)";

                var mat = AppLookupServices.Materials.FindById(id);
                return mat != null ? $"{mat.Name} ({mat.Id})" : id;
            }
        }

        // Called by MainViewModel when selection changes
        public void UpdateSelection(object? selected)
        {
            SelectedObject = selected;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class MemberModel
    {
        private int v;
        private Point3D start;
        private Point3D end;

        public MemberModel(int v, Point3D start, Point3D end)
        {
            this.v = v;
            this.start = start;
            this.end = end;
        }

        public string SectionId { get; internal set; }
        public string MaterialId { get; internal set; }
    }
}
