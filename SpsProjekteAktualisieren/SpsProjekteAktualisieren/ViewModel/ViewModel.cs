namespace SpsProjekteAktualisieren.ViewModel
{
    using SpsProjekteAktualisieren.Commands;
    using System.Windows.Input;

    public class ViewModel
    {

        public Model.ProjekteAktualsieren projekteAktualsieren;
        public VisuAnzeigen ViAnzeige { get; set; }

        public ViewModel()
        {
            projekteAktualsieren = new SpsProjekteAktualisieren.Model.ProjekteAktualsieren();
            ViAnzeige = new VisuAnzeigen(projekteAktualsieren);
        }

        public Model.ProjekteAktualsieren ProjekteAktualsieren { get { return projekteAktualsieren; } }

        #region BtnAktualisieren
        private ICommand _btnAktualisieren;
        public ICommand BtnAktualisieren
        {
            get
            {
                if (_btnAktualisieren == null)
                {
                    _btnAktualisieren = new RelayCommand(p => projekteAktualsieren.AlleAktualisieren(), p => true);
                }
                return _btnAktualisieren;
            }
        }
        #endregion

    }
}
