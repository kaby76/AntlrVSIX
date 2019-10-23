namespace Symtab
{

    /// <summary>
    /// A scope object typically associated with {...} code blocks </summary>
    public class LocalScope : BaseScope
    {
        public LocalScope(IScope enclosingScope) : base(enclosingScope)
        {
        }
    }
}