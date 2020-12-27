namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    /// <summary>
    /// A field symbol is just a variable that lives inside an aggregate like a
    ///  class or struct.
    /// </summary>
    public class FieldSymbol : VariableSymbol, IMemberSymbol
    {
        protected internal int slot;

        public FieldSymbol(string n, IList<IToken> t) : base(n, t)
        {
        }

        public int getSlotNumber()
        {
            return slot;
        }
    }

}