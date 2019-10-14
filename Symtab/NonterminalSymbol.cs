namespace Symtab
{
    using Antlr4.Runtime;

    public class NonterminalSymbol : BaseSymbol, ISymbol
    {
        public NonterminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
