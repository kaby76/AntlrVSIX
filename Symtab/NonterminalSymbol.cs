using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Symtab
{
    public class NonterminalSymbol : BaseSymbol, Symbol
    {
        public NonterminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
