namespace Symtab
{
    using Antlr4.Runtime;

    public class ModeSymbol : BaseSymbol, Symbol
    {
        public ModeSymbol(string n, IToken t) : base(n, t)
        {
        }
    }
}
