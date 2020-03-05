namespace Symtab
{

    /// <summary>
    /// A marginally useful object to track predefined and global scopes. </summary>
    public class SymbolTable
    {
        private readonly bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            GLOBALS = new GlobalScope(PREDEFINED);
        }

        public static readonly IType INVALID_TYPE = new InvalidType();

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

        public virtual void definePredefinedSymbol(ref ISymbol s)
        {
            PREDEFINED.define(ref s);
        }

        public virtual void defineGlobalSymbol(ref ISymbol s)
        {
            GLOBALS.define(ref s);
        }
    }
}