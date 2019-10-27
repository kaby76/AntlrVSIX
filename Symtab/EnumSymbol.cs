namespace Symtab
{
    using Antlr4.Runtime;

    public class EnumSymbol : SymbolWithScope, ISymbol, IType
    {
        public EnumSymbol(string n, IToken t) : base(n, t)
        {
        }

        public int TypeIndex { get; }
    }
}
