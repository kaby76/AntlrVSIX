namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    // Used for variables ref and def in a code block.
    public class LocalSymbol : VariableSymbol
    {
        public LocalSymbol(string n, IList<IToken> t) : base(n, t) { }
    }
}
