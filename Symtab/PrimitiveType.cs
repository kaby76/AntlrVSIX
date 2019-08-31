namespace org.antlr.symtab
{

    public class PrimitiveType : BaseSymbol, Type
    {
        protected internal int typeIndex;

        public PrimitiveType(string name) : base(name)
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