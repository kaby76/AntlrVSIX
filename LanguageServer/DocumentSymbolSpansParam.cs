namespace LanguageServer
{
    public class CustomMessageParams
    {
        public CustomMessageParams() { }
        public System.Uri TextDocument;
        public int Start;
        public int End;
    }

    public class CustomMessage2Params
    {
        public CustomMessage2Params() { }
        public System.Uri TextDocument;
        public int Pos;
        public bool Forward;
    }

    public class CMGotoParams
    {
        public CMGotoParams() { }
        public System.Uri TextDocument;
        public int Pos;
    }

    public class CMGotoResult
    {
        public CMGotoResult() { }
        public System.Uri TextDocument;
        public int Start;
    }
}
