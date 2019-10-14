namespace Symtab
{
    using Antlr4.Runtime;

    /// <summary>
    /// A "typedef int I;" in C results in a TypeAlias("I", ptrToIntegerType) </summary>
    public class TypeAlias : BaseSymbol, IType
    {
        protected internal IType targetType;
        public TypeAlias(string name, IType targetType, IToken token) : base(name, token)
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

        public virtual IType TargetType
        {
            get
            {
                return targetType;
            }
        }
    }

}