using System.Windows;
using CBFrame.App.Wpf.ViewModels;

namespace CBFrame.App.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Hook up the shell view-model
            DataContext = new MainViewModel();
        }
    }
}
