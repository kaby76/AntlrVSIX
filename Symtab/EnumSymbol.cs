namespace Symtab
{
    public class EnumSymbol : SymbolWithScope, ISymbol, IType
    {
        public EnumSymbol(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
