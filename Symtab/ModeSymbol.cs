using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Symtab
{
    public class ModeSymbol : BaseSymbol, Symbol
    {
        public ModeSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
