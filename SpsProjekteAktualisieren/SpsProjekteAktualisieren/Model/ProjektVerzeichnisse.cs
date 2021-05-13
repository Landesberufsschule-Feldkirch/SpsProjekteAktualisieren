using System.Collections.ObjectModel;

namespace SpsProjekteAktualisieren.Model
{
    public class ProjektVerzeichnisse
    {
        public ObservableCollection<Ordner> AlleProjektVerzeichnisse { get; set; } = new();
    }

    public class Ordner
    {
        public Ordner()
        {
            Kommentar = "";
            Quelle = "";
            Ziel = "";
        }
        public string Kommentar { get; set; }
        public string Quelle { get; set; }
        public string Ziel { get; set; }
    }
}
