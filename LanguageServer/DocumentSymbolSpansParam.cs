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

    public class CustomMessage3Params
    {
        public CustomMessage3Params() { }
        public System.Uri TextDocument;
        public int Pos;
    }

    public class CustomMessage3Result
    {
        public CustomMessage3Result() { }
        public System.Uri TextDocument;
        public int Start;
    }
}
