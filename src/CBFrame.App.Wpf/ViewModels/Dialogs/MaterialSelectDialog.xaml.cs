using System;
using System.Windows;
using CBFrame.App.Wpf.ViewModels.Dialogs;
using CBFrame.MaterialsDb.Models;

namespace CBFrame.App.Wpf.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for MaterialSelectDialog.xaml
    /// </summary>
    public partial class MaterialSelectDialog : Window
    {
        public MaterialRecord? SelectedMaterial =>
            (DataContext as MaterialSelectDialogViewModel)?.SelectedMaterial;

        public MaterialSelectDialog()
        {
            InitializeComponent();

            var vm = new MaterialSelectDialogViewModel();
            vm.RequestCloseOk += OnRequestCloseOk;
            vm.RequestCloseCancel += OnRequestCloseCancel;

            DataContext = vm;
        }

        private void OnRequestCloseOk(object? sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnRequestCloseCancel(object? sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
