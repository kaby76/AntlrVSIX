using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class NonterminalSymbol : BaseSymbol, Symbol
    {
        public NonterminalSymbol(string n, int l, int c, string f) : base(n, l, c, f)
        {
        }
    }
}
