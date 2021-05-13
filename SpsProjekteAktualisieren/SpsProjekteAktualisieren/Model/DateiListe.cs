using System.Collections.ObjectModel;

namespace SpsProjekteAktualisieren.Model
{
    public class DateiListe
    {
        public ObservableCollection<DateiEigenschaften> AlleDateien { get; set; } = new();
    }
    public class DateiEigenschaften
    {
        public string Beschreibung { get; set; }
        public string Dateiname { get; set; }
        public DateiEigenschaften()
        {
            Beschreibung = "";
            Dateiname = "";
        }
    }
}