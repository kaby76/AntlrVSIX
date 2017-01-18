namespace AntlrVSIX.Model
{
    using Antlr4.Runtime;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class Entry : INotifyPropertyChanged
    {
        public Entry()
        {
        }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
        public IToken Token { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
