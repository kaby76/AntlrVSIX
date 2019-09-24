namespace Symtab
{

    /// <summary>
    /// A parameter is just kind of variable used as an argument to a
    ///  function or method.
    /// </summary>
    public class ParameterSymbol : VariableSymbol
    {
        public ParameterSymbol(string n, int l, int c, string f) : base(n, l, c, f)
        {
        }
    }

}