using System;
using System.IO;
using System.Text;

namespace SpsProjekteAktualisieren.Model
{
    public class ProjekteAktualsieren
    {
        public DateiListe AlleDateiListen { get; set; }
        public ProjektVerzeichnisse OrdnerStruktur { get; set; }
        public string QuellOrdner { get; set; }
        public string ZielOrdner { get; set; }


        private readonly StringBuilder _textBoxText;
        private System.Collections.Generic.IEnumerable<string> _fileNames;

        const int BYTES_TO_READ = sizeof(Int64);

        public ProjekteAktualsieren()
        {
            AlleDateiListen = Newtonsoft.Json.JsonConvert.DeserializeObject<DateiListe>(File.ReadAllText("DateiListen.json"));

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
                var Quelle = QuellOrdner + "/" + struktur.Quelle;
                var Ziel = ZielOrdner + "/" + struktur.Ziel;

                foreach (var datei in AlleDateiListen.AlleDateien)
                {
                    if (!FilesAreEqual(new FileInfo(Quelle + "/" + datei.Dateiname), new FileInfo(datei.Dateiname)))
                    {
                        _textBoxText.Clear();
                        _textBoxText.Append("IP Adressen sind unterschiedlich eingestellt! \n");
                    }
                    _textBoxText.Append(Quelle + "\n");
                    _textBoxText.Append(Ziel + "\n\n");
                }
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



        static public bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0)) return false;
                }
            }

            return true;
        }
    }
}