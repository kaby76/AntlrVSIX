using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Symtab
{
    public class TerminalSymbol : BaseSymbol, Symbol
    {
        public TerminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
