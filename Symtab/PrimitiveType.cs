namespace Symtab
{

    public class PrimitiveType : BaseSymbol, Type
    {
        protected internal int typeIndex;

        public PrimitiveType(string n, int l, int c, string f) : base(n, l, c, f)
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