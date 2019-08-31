namespace org.antlr.symtab
{

    /// <summary>
    /// A scope associated with globals. </summary>
    public class GlobalScope : BaseScope
    {
        public GlobalScope(Scope scope) : base(scope)
        {
        }
        public override string Name
        {
            get
            {
                return "global";
            }
        }
    }

}