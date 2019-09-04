namespace Symtab
{
    // Used for variables ref and def in a code block.
    public class LocalSymbol : VariableSymbol
    {
        public LocalSymbol(string name) : base(name) { }
    }
}
