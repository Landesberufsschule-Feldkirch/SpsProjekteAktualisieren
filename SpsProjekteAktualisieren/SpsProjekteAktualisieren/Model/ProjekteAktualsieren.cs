using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace SpsProjekteAktualisieren.Model
{
    public class ProjekteAktualsieren
    {
        public ProjektVerzeichnisse OrdnerStruktur { get; set; }
        public string QuellOrdner { get; set; }
        public string ZielOrdner { get; set; }


        private readonly StringBuilder textBoxText;
        private System.Collections.Generic.IEnumerable<string> fileNames;

        public ProjekteAktualsieren()
        {
            textBoxText = new StringBuilder();
            OrdnerStruktur = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjektVerzeichnisse>(File.ReadAllText("ProjektVerzeichnisse.json"));
            if (OrdnerStruktur.AlleProjektVerzeichnisse[0].Kommentar == "Ordner")
            {
                QuellOrdner = OrdnerStruktur.AlleProjektVerzeichnisse[0].Quelle;
                ZielOrdner = OrdnerStruktur.AlleProjektVerzeichnisse[0].Ziel;
            }

            textBoxText.Clear();
            foreach (var struktur in OrdnerStruktur.AlleProjektVerzeichnisse)
            {
                if (struktur.Kommentar != "Ordner")
                {
                    textBoxText.Append(QuellOrdner + "/" + struktur.Quelle + "\n");
                    textBoxText.Append(ZielOrdner + "/" + struktur.Ziel + "\n\n");
                }
            }
        }

        internal void AlleAktualisieren()
        {
            textBoxText.Clear();
            foreach (var struktur in OrdnerStruktur.AlleProjektVerzeichnisse)
            {
                if (struktur.Kommentar != "Ordner")
                {
                    DateienAktualisieren(QuellOrdner + "/" + struktur.Quelle, ZielOrdner + "/" + struktur.Ziel);
                }
            }
        }

        internal void DateienAktualisieren(string quelle, string ziel)
        {
            var laengeQuelle = quelle.Length;


            try
            {
                fileNames = Directory.EnumerateFiles(quelle, "*.*", SearchOption.AllDirectories)
                           .Where(s => s.EndsWith(".exe") || s.EndsWith(".dll") || s.EndsWith(".json")
                           );
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Failed to open file - " + e.ToString());
            }

            textBoxText.Append(quelle + "\n");


            foreach (var quellName in fileNames)
            {
                var zielName = ziel + "/" + quellName.Substring(laengeQuelle + 1);
                try
                {
                    File.Copy(quellName, zielName, true);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("Failed to open file - " + e.ToString());
                }

                textBoxText.Append(zielName + "\n");
            }

            textBoxText.Append("\n");
        }

        internal StringBuilder TextBoxText() => textBoxText;
    }
}
