namespace org.antlr.symtab
{
    using Antlr4.Runtime;

    /// <summary>
    /// This symbol represents a function ala C, not a method ala Java.
    ///  You can associate a node in the parse tree that is responsible
    ///  for defining this symbol.
    /// </summary>
    public class FunctionSymbol : SymbolWithScope, TypedSymbol
    {
        protected internal ParserRuleContext defNode;
        protected internal Type retType;

        public FunctionSymbol(string name) : base(name)
        {
        }

        public virtual ParserRuleContext DefNode
        {
            set
            {
                this.defNode = value;
            }
            get
            {
                return defNode;
            }
        }


        public virtual Type Type
        {
            get
            {
                return retType;
            }
            set
            {
                retType = value;
            }
        }


        /// <summary>
        /// Return the number of VariableSymbols specifically defined in the scope.
        ///  This is useful as either the number of parameters or the number of
        ///  parameters and locals depending on how you build the scope tree.
        /// </summary>
        public virtual int NumberOfVariables
        {
            get
            {
                return Utils.filter(symbols.Values, s => s is VariableSymbol).Count;
            }
        }

        public virtual int NumberOfParameters
        {
            get
            {
                return Utils.filter(symbols.Values, s => s is ParameterSymbol).Count;
            }
        }

        public override string ToString()
        {
            return name + ":" + base.ToString();
        }
    }

}