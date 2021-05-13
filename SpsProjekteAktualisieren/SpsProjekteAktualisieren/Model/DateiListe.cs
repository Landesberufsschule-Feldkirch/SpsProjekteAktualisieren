using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SpsProjekteAktualisieren.Model
{

    public class DateiListe
    {
        public ObservableCollection<DateiEigenschaften> AlleDateien { get; set; } = new ObservableCollection<DateiEigenschaften>();
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
