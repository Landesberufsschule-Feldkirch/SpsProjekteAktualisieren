namespace SpsProjekteAktualisieren.ViewModel
{
    using Commands;
    using System.Windows.Input;

    public class ViewModel
    {
        public Model.ProjekteAktualsieren ProjekteAktualsieren { get; }
        public VisuAnzeigen ViAnzeige { get; set; }

        public ViewModel()
        {
            ProjekteAktualsieren = new Model.ProjekteAktualsieren();
            ViAnzeige = new VisuAnzeigen(ProjekteAktualsieren);
        }

        private ICommand _btnAktualisieren;
        // ReSharper disable once UnusedMember.Global
        public ICommand BtnAktualisieren => _btnAktualisieren ??= new RelayCommand(_ => ProjekteAktualsieren.AlleAktualisieren(), _ => true);
    }
}