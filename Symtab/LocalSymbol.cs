namespace Symtab
{
    using Antlr4.Runtime;

    // Used for variables ref and def in a code block.
    public class LocalSymbol : VariableSymbol
    {
        public LocalSymbol(string n, IToken t) : base(n, t) { }
    }
}
