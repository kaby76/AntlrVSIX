using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class TerminalSymbol : BaseSymbol, Symbol
    {
        public TerminalSymbol(string name) : base(name)
        {
        }
    }
}
