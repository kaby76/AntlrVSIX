namespace Symtab
{
    public class DirectoryScope : BaseScope
    {
        public DirectoryScope(string name, IScope enclosingScope) : base(enclosingScope)
        {
            Name = name;
        }
    }
}
