namespace Symtab
{
    using Antlr4.Runtime;

    public class NonterminalSymbol : BaseSymbol, Symbol
    {
        public NonterminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
