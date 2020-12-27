namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class StructSymbol : DataAggregateSymbol
    {
        public StructSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }
    }

}