namespace Symtab
{
    using Antlr4.Runtime;

    /// <summary>
    /// This symbol represents a function ala C, not a method ala Java.
    ///  You can associate a node in the parse tree that is responsible
    ///  for defining this symbol.
    /// </summary>
    public class FunctionSymbol : SymbolWithScope, ITypedSymbol
    {
        protected internal ParserRuleContext defNode;
        protected internal IType retType;

        public FunctionSymbol(string n, IToken t) : base(n, t)
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


        public virtual IType Type
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
            return Name + ":" + base.ToString();
        }
    }

}