namespace Symtab
{

    /// <summary>
    /// A scope associated with globals. </summary>
    public class GlobalScope : BaseScope
    {
        public GlobalScope(IScope scope) : base(scope)
        {
        }
    }
}