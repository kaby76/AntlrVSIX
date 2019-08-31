namespace org.antlr.symtab
{

    /// <summary>
    /// This interface tags user-defined symbols that have static type information,
    ///  like variables and functions.
    /// </summary>
    public interface TypedSymbol
    {
        Type Type {get;set;}
    }

}