namespace Symtab
{
    public class EnumSymbol : SymbolWithScope, Symbol, Type
    {
        public EnumSymbol(string name) : base(name)
        {
        }

        public int TypeIndex { get; }
    }
}
