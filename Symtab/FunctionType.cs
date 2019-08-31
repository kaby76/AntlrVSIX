namespace org.antlr.symtab
{
    using System.Collections.Generic;

    /// <summary>
    /// For C types like "void (*)(int)", we need that to be a pointer to a function
    ///  taking a single integer argument returning void.
    /// </summary>
    public class FunctionType : Type
    {
        protected internal readonly Type returnType;
        protected internal readonly IList<Type> argumentTypes;

        public FunctionType(Type returnType, IList<Type> argumentTypes)
        {
            this.returnType = returnType;
            this.argumentTypes = argumentTypes;
        }

        public virtual string Name
        {
            get
            {
                return ToString();
            }
        }

        public virtual int TypeIndex
        {
            get
            {
                return -1;
            }
        }

        public virtual IList<Type> ArgumentTypes
        {
            get
            {
                return argumentTypes;
            }
        }

        public override string ToString()
        {
            return "*" + returnType;
        }
    }

}