namespace org.antlr.symtab
{

    /// <summary>
    /// A marginally useful object to track predefined and global scopes. </summary>
    public class SymbolTable
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            GLOBALS = new GlobalScope(PREDEFINED);
        }

        public static readonly Type INVALID_TYPE = new InvalidType();

        public BaseScope PREDEFINED = new PredefinedScope();
        public GlobalScope GLOBALS = new GlobalScope(null);

        public SymbolTable()
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }

        public virtual void initTypeSystem()
        {
        }

        public virtual void definePredefinedSymbol(Symbol s)
        {
            PREDEFINED.define(s);
        }

        public virtual void defineGlobalSymbol(Symbol s)
        {
            GLOBALS.define(s);
        }
    }
}