namespace org.antlr.symtab
{

    /// <summary>
    /// A generic programming language symbol. A symbol has to have a name and
    ///  a scope in which it lives. It also helps to know the order in which
    ///  symbols are added to a scope because this often translates to
    ///  register or parameter numbers.
    /// </summary>
    public interface Symbol
    {
        string Name {get;}
        Scope Scope {get;set;}
        int InsertionOrderNumber {get;set;}

        // to satisfy adding symbols to sets, hashtables
        int GetHashCode();
        bool Equals(object o);
    }

}