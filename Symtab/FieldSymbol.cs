namespace Symtab
{
    using Antlr4.Runtime;

    /// <summary>
    /// A field symbol is just a variable that lives inside an aggregate like a
    ///  class or struct.
    /// </summary>
    public class FieldSymbol : VariableSymbol, MemberSymbol
    {
        protected internal int slot;

        public FieldSymbol(string n, IToken t) : base(n, t)
        {
        }

        public int getSlotNumber()
        {
            return slot;
        }
    }

}