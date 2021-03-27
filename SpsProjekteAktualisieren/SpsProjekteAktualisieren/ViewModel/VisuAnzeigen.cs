namespace SpsProjekteAktualisieren.ViewModel
{
    using System.ComponentModel;
    using System.Threading;

    public class VisuAnzeigen : INotifyPropertyChanged
    {
        private readonly Model.ProjekteAktualsieren _projekteAktualsieren;

        public VisuAnzeigen(Model.ProjekteAktualsieren pA)
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

                Thread.Sleep(10);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        #region TextBoxText
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
        #endregion


        #region iNotifyPeropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion iNotifyPeropertyChanged Members
    }
}