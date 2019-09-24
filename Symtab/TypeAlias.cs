namespace Symtab
{

    /// <summary>
    /// A "typedef int I;" in C results in a TypeAlias("I", ptrToIntegerType) </summary>
    public class TypeAlias : BaseSymbol, Type
    {
        protected internal Type targetType;
        public TypeAlias(string name, Type targetType, int l, int c, string f) : base(name, l, c, f)
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

        public virtual Type TargetType
        {
            get
            {
                return targetType;
            }
        }
    }

}