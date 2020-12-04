namespace SpsProjekteAktualisieren
{
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow
    {
        public MainWindow()
        {
            var viewModel = new ViewModel.ViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
