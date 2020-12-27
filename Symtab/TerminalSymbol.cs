namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class TerminalSymbol : BaseSymbol, ISymbol
    {
        public TerminalSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }
    }
}
