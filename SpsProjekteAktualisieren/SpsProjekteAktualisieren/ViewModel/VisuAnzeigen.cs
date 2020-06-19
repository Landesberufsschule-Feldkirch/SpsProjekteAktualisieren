namespace SpsProjekteAktualisieren.ViewModel
{
    using System.ComponentModel;
    using System.Threading;

    public class VisuAnzeigen : INotifyPropertyChanged
    {
        private readonly Model.ProjekteAktualsieren projekteAktualsieren;

        public VisuAnzeigen(Model.ProjekteAktualsieren pA)
        {
            projekteAktualsieren = pA;
            TextBoxText = "";

            System.Threading.Tasks.Task.Run(() => VisuAnzeigenTask());
        }

        private void VisuAnzeigenTask()
        {
            while (true)
            {
                TextBoxText = projekteAktualsieren.TextBoxText();

                Thread.Sleep(10);
            }
        }

        #region TextBoxText
        private object _textBoxText;
        public object TextBoxText
        {
            get { return _textBoxText; }
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