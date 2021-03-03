using System.IO;
using System.Text;

namespace SpsProjekteAktualisieren.Model
{
    public class ProjekteAktualsieren
    {
        public ProjektVerzeichnisse OrdnerStruktur { get; set; }
        public string QuellOrdner { get; set; }
        public string ZielOrdner { get; set; }

        private readonly StringBuilder _textBoxText;

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
                    DirectoryCopy(QuellOrdner + "/" + struktur.Quelle, ZielOrdner + "/" + struktur.Ziel);
                }
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        internal StringBuilder TextBoxText() => _textBoxText;
    }
}