namespace Symtab
{

    public class PrimitiveType : BaseSymbol, TypeReference
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