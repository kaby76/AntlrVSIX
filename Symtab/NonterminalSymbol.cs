namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class NonterminalSymbol : BaseSymbol, ISymbol
    {
        public NonterminalSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }
    }
}
