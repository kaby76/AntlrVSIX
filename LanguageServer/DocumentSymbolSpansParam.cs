using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

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
}
