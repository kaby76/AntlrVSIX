namespace Symtab
{
    using Antlr4.Runtime;

    public class TerminalSymbol : BaseSymbol, Symbol
    {
        public TerminalSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
