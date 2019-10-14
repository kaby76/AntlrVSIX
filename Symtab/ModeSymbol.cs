namespace Symtab
{
    using Antlr4.Runtime;

    public class ModeSymbol : BaseSymbol, ISymbol
    {
        public ModeSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
