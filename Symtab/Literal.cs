namespace Symtab
{
    public class Literal : SymbolWithScope, Symbol
    {
        public Literal(string value, string fixed_value, int ind) : base(value)
        {
            index = ind;
            Cleaned = fixed_value;
        }

        public int TypeIndex { get; }

        public string Cleaned { get; protected set; }
    }
}
