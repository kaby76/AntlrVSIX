namespace Symtab
{

    public class PrimitiveType : BaseSymbol, Type
    {
        public Mono.Cecil.MemberReference MonoType { get; set; }
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