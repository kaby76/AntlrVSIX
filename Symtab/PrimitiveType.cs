namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class PrimitiveType : BaseSymbol, IType
    {
        protected internal int typeIndex;

        public PrimitiveType(string n, IList<IToken> t) : base(n, t)
        {
        }

        public virtual int TypeIndex
        {
            get => typeIndex;
            set => typeIndex = value;
        }
    }
}