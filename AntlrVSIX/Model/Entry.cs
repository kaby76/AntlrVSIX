namespace AntlrVSIX.Model
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Entry : INotifyPropertyChanged
    {
        public Entry() { }
        public string FileName { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
