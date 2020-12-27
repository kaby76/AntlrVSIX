namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    /// <summary>
    /// A "typedef int I;" in C results in a TypeAlias("I", ptrToIntegerType) </summary>
    public class TypeAlias : BaseSymbol, IType
    {
        protected internal IType targetType;
        public TypeAlias(string name, IType targetType, IList<IToken> token) : base(name, token)
        {
            this.targetType = targetType;
        }

        public virtual int TypeIndex => -1;

        public virtual IType TargetType => targetType;
    }

}