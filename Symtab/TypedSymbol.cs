namespace Symtab
{

    /// <summary>
    /// This interface tags user-defined symbols that have static type information,
    ///  like variables and functions.
    /// </summary>
    public interface TypedSymbol
    {
        TypeReference Type {get;set;}
    }

}