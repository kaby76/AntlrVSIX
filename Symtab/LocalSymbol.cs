namespace Symtab
{
    // Used for variables ref and def in a code block.
    public class LocalSymbol : VariableSymbol
    {
        public LocalSymbol(string n, int l, int c, string f) : base(n, l, c, f) { }
    }
}
