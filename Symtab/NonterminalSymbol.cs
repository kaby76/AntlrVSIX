using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class NonterminalSymbol : BaseSymbol, Symbol
    {
        public NonterminalSymbol(string name) : base(name)
        {
        }
    }
}
