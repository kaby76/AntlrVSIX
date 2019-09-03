namespace Symtab
{

    /// <summary>
    /// A "typedef int I;" in C results in a TypeAlias("I", ptrToIntegerType) </summary>
    public class TypeAlias : BaseSymbol, TypeReference
    {
        protected internal TypeReference targetType;
        public TypeAlias(string name, TypeReference targetType) : base(name)
        {
            this.targetType = targetType;
        }

        public virtual int TypeIndex
        {
            get
            {
                return -1;
            }
        }

        public virtual TypeReference TargetType
        {
            get
            {
                return targetType;
            }
        }
    }

}