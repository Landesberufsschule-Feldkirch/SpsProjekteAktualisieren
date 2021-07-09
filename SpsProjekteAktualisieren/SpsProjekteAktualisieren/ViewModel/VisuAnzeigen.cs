using System.Windows.Media;
using SpsProjekteAktualisieren.Model;

namespace SpsProjekteAktualisieren.ViewModel
{
    using System.ComponentModel;
    using System.Threading;

    public class VisuAnzeigen : INotifyPropertyChanged
    {
        private readonly ProjekteAktualsieren _projekteAktualsieren;

        public VisuAnzeigen(ProjekteAktualsieren pA)
        {
            _projekteAktualsieren = pA;
            TextBoxText = "";

            System.Threading.Tasks.Task.Run(VisuAnzeigenTask);
        }
        private void VisuAnzeigenTask()
        {
            while (true)
            {
                TextBoxText = _projekteAktualsieren.TextBoxText();
                HintergrundFarbe = _projekteAktualsieren.HintergrundFarbe();

                Thread.Sleep(10);
            }
            // ReSharper disable once FunctionNeverReturns
        }
        private object _textBoxText;
        public object TextBoxText
        {
            get => _textBoxText;
            set
            {
                _textBoxText = value;
                OnPropertyChanged("TextBoxText");
            }
        }

        private Brush _hintergrundFarbe;
        public Brush HintergrundFarbe
        {
            get => _hintergrundFarbe;
            set
            {
                _hintergrundFarbe = value;
                OnPropertyChanged("HintergrundFarbe");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}