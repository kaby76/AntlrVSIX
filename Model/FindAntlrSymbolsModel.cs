namespace AntlrVSIX.Model
{
    using Antlr4.Runtime;
    using AntlrVSIX.Extensions;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal class FindAntlrSymbolsModel : INotifyPropertyChanged
    {
        public static FindAntlrSymbolsModel Instance { get; } = new FindAntlrSymbolsModel();

        public ObservableCollection<Entry> Results{
            get; set; } = new ObservableCollection<Entry>();

        private FindAntlrSymbolsModel() { }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Entry _item_selected = null;
        public Entry ItemSelected
        {
            get { return _item_selected; }
            set
            {
                _item_selected = value;
                if (value == null) return;
                string full_file_name = _item_selected.FileName;
                IVsTextView vstv = IVsTextViewExtensions.GetIVsTextView(full_file_name);
                IVsTextViewExtensions.ShowFrame(full_file_name);
                vstv = IVsTextViewExtensions.GetIVsTextView(full_file_name);
                IWpfTextView wpftv = vstv.GetIWpfTextView();
                if (wpftv == null) return;
                int line_number = _item_selected.LineNumber;
                int colum_number = _item_selected.ColumnNumber;
                IToken token = _item_selected.Token;
                // Create new span in the appropriate view.
                ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                SnapshotSpan ss = new SnapshotSpan(cc, token.StartIndex, 1);
                SnapshotPoint sp = ss.Start;
                // Put cursor on symbol.
                wpftv.Caret.MoveTo(sp);     // This sets cursor, bot does not center.
                                            // Center on cursor.
                                            //wpftv.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!
                if (line_number > 0)
                    vstv.CenterLines(line_number - 1, 2);
                else
                    vstv.CenterLines(line_number, 1);
            }
        }
    }
}
