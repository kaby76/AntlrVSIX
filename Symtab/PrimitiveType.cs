namespace Symtab
{
    using Antlr4.Runtime;

    public class PrimitiveType : BaseSymbol, Type
    {
        protected internal int typeIndex;

        public PrimitiveType(string n, IToken t) : base(n, t)
        {
        }

        public virtual int TypeIndex
        {
            get
            {
                return typeIndex;
            }
            set
            {
                this.typeIndex = value;
            }
        }


        public override string Name
        {
            get
            {
                return name;
            }
        }
    }

}