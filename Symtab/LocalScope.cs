namespace org.antlr.symtab
{

    /// <summary>
    /// A scope object typically associated with {...} code blocks </summary>
    public class LocalScope : BaseScope
    {
        public LocalScope(Scope enclosingScope) : base(enclosingScope)
        {
        }

        public override string Name
        {
            get
            {
                return "local";
            }
        }
    }

}