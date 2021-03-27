using System;
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


        private readonly StringBuilder _textBoxText;
        private System.Collections.Generic.IEnumerable<string> _fileNames;

        public ProjekteAktualsieren()
        {
            _textBoxText = new StringBuilder();
            OrdnerStruktur = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjektVerzeichnisse>(File.ReadAllText("ProjektVerzeichnisse.json"));
            if (OrdnerStruktur.AlleProjektVerzeichnisse[0].Kommentar == "Ordner")
            {
                QuellOrdner = OrdnerStruktur.AlleProjektVerzeichnisse[0].Quelle;
                ZielOrdner = OrdnerStruktur.AlleProjektVerzeichnisse[0].Ziel;
            }

            _textBoxText.Clear();
            foreach (var struktur in OrdnerStruktur.AlleProjektVerzeichnisse)
            {
                if (struktur.Kommentar == "Ordner") continue;
                _textBoxText.Append(QuellOrdner + "/" + struktur.Quelle + "\n");
                _textBoxText.Append(ZielOrdner + "/" + struktur.Ziel + "\n\n");
            }
        }

        internal void AlleAktualisieren()
        {
            _textBoxText.Clear();
            foreach (var struktur in OrdnerStruktur.AlleProjektVerzeichnisse)
            {
                if (struktur.Kommentar != "Ordner")
                {
                    OrdnerLoeschen(ZielOrdner + "/" + struktur.Ziel);
                    DateienAktualisieren(QuellOrdner + "/" + struktur.Quelle, ZielOrdner + "/" + struktur.Ziel);
                }
            }
        }

        private void OrdnerLoeschen(string ordner)
        {
            _textBoxText.Append("Ordner löschen: " + ordner + "\n");
            if (Directory.Exists(ordner)) Directory.Delete(ordner, true);
        }

        internal void DateienAktualisieren(string quelle, string ziel)
        {
            _textBoxText.Append("Ordner aktualisieren: " + ziel + "\n\n");
            DirectoryCopy(quelle, ziel, true);
        }


        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        internal StringBuilder TextBoxText() => _textBoxText;
    }
}