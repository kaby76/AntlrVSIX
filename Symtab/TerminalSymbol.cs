namespace Symtab
{
    using Antlr4.Runtime;

    public class TerminalSymbol : BaseSymbol, ISymbol
    {
        public TerminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
