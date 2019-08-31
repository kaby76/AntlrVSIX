namespace org.antlr.symtab
{

    /// <summary>
    /// A field symbol is just a variable that lives inside an aggregate like a
    ///  class or struct.
    /// </summary>
    public class FieldSymbol : VariableSymbol, MemberSymbol
    {
        protected internal int slot;

        public FieldSymbol(string name) : base(name)
        {
        }

        public int getSlotNumber()
        {
            return slot;
        }
    }

}