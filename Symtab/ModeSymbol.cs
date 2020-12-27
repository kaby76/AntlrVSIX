namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class ModeSymbol : BaseSymbol, ISymbol
    {
        public ModeSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }
    }
}
