namespace LanguageServer
{
    public class CMGetClassifiersParams
    {
        public CMGetClassifiersParams() { }
        public System.Uri TextDocument;
        public int Start;
        public int End;
    }

    public class CMNextSymbolParams
    {
        public CMNextSymbolParams() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool Forward;
    }

    public class CMGotoParams
    {
        public CMGotoParams() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool IsEnter;
    }

    public class CMGotoResult
    {
        public CMGotoResult() { }
        public System.Uri TextDocument;
        public int Start;
    }
}
