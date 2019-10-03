namespace Symtab
{
    using Antlr4.Runtime;

    /// <summary>
    /// A parameter is just kind of variable used as an argument to a
    ///  function or method.
    /// </summary>
    public class ParameterSymbol : VariableSymbol
    {
        public ParameterSymbol(string n, IToken t) : base(n, t)
        {
        }
    }

}