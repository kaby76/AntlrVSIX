using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class TerminalSymbol : BaseSymbol, Symbol
    {
        public TerminalSymbol(string n, int l, int c, string f) : base(n, l, c, f)
        {
        }
    }
}
