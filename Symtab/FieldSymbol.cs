namespace Symtab
{

    /// <summary>
    /// A field symbol is just a variable that lives inside an aggregate like a
    ///  class or struct.
    /// </summary>
    public class FieldSymbol : VariableSymbol, MemberSymbol
    {
        protected internal int slot;

        public FieldSymbol(string n, int l, int c, string f) : base(n, l, c, f)
        {
        }

        public int getSlotNumber()
        {
            return slot;
        }
    }

}