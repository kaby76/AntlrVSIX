namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    /// <summary>
    /// A method symbol is a function that lives within an aggregate/class and has a slot number. </summary>
    public class MethodSymbol : FunctionSymbol, IMemberSymbol
    {
        protected internal int slot = -1;

        public MethodSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }

        public int getSlotNumber()
        {
            return slot;
        }
    }

}