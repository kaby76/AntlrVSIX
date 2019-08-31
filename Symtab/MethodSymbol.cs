namespace org.antlr.symtab
{

    /// <summary>
    /// A method symbol is a function that lives within an aggregate/class and has a slot number. </summary>
    public class MethodSymbol : FunctionSymbol, MemberSymbol
    {
        protected internal int slot = -1;

        public MethodSymbol(string name) : base(name)
        {
        }

        public int getSlotNumber()
        {
                return slot;
        }
    }

}