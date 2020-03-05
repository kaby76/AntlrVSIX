namespace Symtab
{

    /// <summary>
    /// An element within a type type such is used in C or Java where we need to
    ///  indicate the type is an array of some element type like float[] or User[].
    ///  It also tracks the size as some types indicate the size of the array.
    /// </summary>
    public class ArrayType : IType
    {
        protected internal readonly IType elemType;
        protected internal readonly int numElems; // some languages allow you to point at arrays of a specific size

        public ArrayType(IType elemType)
        {
            this.elemType = elemType;
            numElems = -1;
        }

        public ArrayType(IType elemType, int numElems)
        {
            this.elemType = elemType;
            this.numElems = numElems;
        }

        public virtual string Name => ToString();

        public virtual int TypeIndex => -1;
        public override string ToString()
        {
            return elemType + "[]";
        }
    }

}