namespace Symtab
{
    using System.Collections.Generic;

    /// <summary>
    /// For C types like "void (*)(int)", we need that to be a pointer to a function
    ///  taking a single integer argument returning void.
    /// </summary>
    public class FunctionType : TypeReference
    {
        protected internal readonly TypeReference returnType;
        protected internal readonly IList<TypeReference> argumentTypes;

        public FunctionType(TypeReference returnType, IList<TypeReference> argumentTypes)
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

        public virtual IList<TypeReference> ArgumentTypes
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