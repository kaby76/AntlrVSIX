namespace Symtab
{
    public class NamespaceScope : BaseScope
    {
        public NamespaceScope(string name, IScope enclosingScope) : base(enclosingScope)
        {
            Name = name;
        }
    }
}
