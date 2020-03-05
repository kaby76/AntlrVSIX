namespace Symtab
{

    /// <summary>
    /// This interface tags user-defined symbols that have static type information,
    ///  like variables and functions.
    /// </summary>
    public interface ITypedSymbol
    {
        IType Type { get; set; }
    }

}