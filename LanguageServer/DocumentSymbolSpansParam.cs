using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LanguageServer
{
    public class DocumentSymbolSpansParams
    {
        public DocumentSymbolSpansParams() { }
        public System.Uri TextDocument;
        public int Start;
        public int End;
    }
}
