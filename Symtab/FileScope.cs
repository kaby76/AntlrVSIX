namespace Symtab
{
    public class FileScope : BaseScope
    {
        public FileScope(string name, IScope enclosingScope) : base(enclosingScope)
        {
            Name = name;
        }
    }
}
