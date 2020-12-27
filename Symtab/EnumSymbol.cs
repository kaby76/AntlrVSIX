namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class EnumSymbol : SymbolWithScope, ISymbol, IType
    {
        public EnumSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }

        public int TypeIndex { get; }
    }
}
