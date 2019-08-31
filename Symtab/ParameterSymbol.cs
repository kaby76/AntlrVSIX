namespace org.antlr.symtab
{

    /// <summary>
    /// A parameter is just kind of variable used as an argument to a
    ///  function or method.
    /// </summary>
    public class ParameterSymbol : VariableSymbol
    {
        public ParameterSymbol(string name) : base(name)
        {
        }
    }

}