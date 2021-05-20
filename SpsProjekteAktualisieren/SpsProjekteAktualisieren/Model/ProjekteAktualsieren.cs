using System;
using System.IO;
using System.Text;
using System.Windows;

namespace SpsProjekteAktualisieren.Model
{
    public class ProjekteAktualsieren
    {
        public DateiListe AlleDateiListen { get; set; }
        public ProjektVerzeichnisse OrdnerStruktur { get; set; }
        public string QuellOrdner { get; set; }
        public string ZielOrdner { get; set; }

        private readonly StringBuilder _textBoxText;
        private const int BytesToRead = sizeof(long);

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
                var quelle = QuellOrdner + "/" + struktur.Quelle;
                var ziel = ZielOrdner + "/" + struktur.Ziel;

                if (!Directory.Exists(quelle))
                {
                    MessageBox.Show("Ordner nicht gefunden:" + quelle);
                    break;
                }

                if (!Directory.Exists(ziel))
                {
                    MessageBox.Show("Ordner nicht gefunden:" + ziel);
                    break;
                }


                foreach (var datei in AlleDateiListen.AlleDateien)
                {
                    if (!FilesAreEqual(new FileInfo(quelle + "/" + datei.Dateiname), new FileInfo(datei.Dateiname)))
                    {
                        _textBoxText.Clear();
                        _textBoxText.Append("IP Adressen sind unterschiedlich eingestellt! \n");
                    }
                    _textBoxText.Append(quelle).Append('\n');
                    _textBoxText.Append(ziel).Append("\n\n");
                }
            }
        }
        internal void AlleAktualisieren()
        {
            _textBoxText.Clear();
            foreach (var struktur in OrdnerStruktur.AlleProjektVerzeichnisse)
            {
                if (struktur.Kommentar == "Ordner") continue;

                OrdnerLoeschen(ZielOrdner + "/" + struktur.Ziel);
                DateienAktualisieren(QuellOrdner + "/" + struktur.Quelle, ZielOrdner + "/" + struktur.Ziel);
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
            DirectoryCopy(quelle, ziel);
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

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }

        }
        internal StringBuilder TextBoxText() => _textBoxText;
        public static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            var iterations = (int)Math.Ceiling((double)first.Length / BytesToRead);

            using var fs1 = first.OpenRead();
            using var fs2 = second.OpenRead();
            var one = new byte[BytesToRead];
            var two = new byte[BytesToRead];

            for (var i = 0; i < iterations; i++)
            {
                fs1.Read(one, 0, BytesToRead);
                fs2.Read(two, 0, BytesToRead);

                if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0)) return false;
            }

            return true;
        }
    }
}