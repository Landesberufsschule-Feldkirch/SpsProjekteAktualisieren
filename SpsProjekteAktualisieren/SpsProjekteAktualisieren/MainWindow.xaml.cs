using System.Windows;

namespace SpsProjekteAktualisieren
{
    public partial class MainWindow : Window
    {
        private readonly ViewModel.ViewModel viewModel;

        public MainWindow()
        {
            viewModel = new ViewModel.ViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
