using System;
using System.Windows;
using CBFrame.App.Wpf.ViewModels.Dialogs;
using CBFrame.SectionsDb.Models;

namespace CBFrame.App.Wpf.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SectionSelectDialog.xaml
    /// </summary>
    public partial class SectionSelectDialog : Window
    {
        public SectionShape? SelectedSection =>
            (DataContext as SectionSelectDialogViewModel)?.SelectedSection;

        public SectionSelectDialog()
        {
            InitializeComponent();

            var vm = new SectionSelectDialogViewModel();
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
