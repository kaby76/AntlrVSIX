namespace Symtab
{
    using Antlr4.Runtime;

    public class StructSymbol : DataAggregateSymbol
    {
        public StructSymbol(string n, IToken t) : base(n, t)
        {
        }
    }

}