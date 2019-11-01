namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract base class that houses common functionality for
    ///  symbols like classes and functions that are both symbols and scopes.
    ///  There is some common cut and paste functionality with <seealso cref="BaseSymbol"/>
    ///  because of a lack of multiple inheritance in Java but it is minimal.
    /// </summary>
    public abstract class SymbolWithScope : BaseScope, ISymbol, IScope
    {
        protected internal int index; // insertion order from 0; compilers often need this

        public SymbolWithScope(string name, IToken token)
        {
            this.Name = name;
            this.Token = token;
        }

        public override string Name { get; set; }

        public virtual IScope Scope
        {
            get
            {
                return enclosingScope;
            }
            set
            {
                EnclosingScope = value;
            }
        }

        public override IScope EnclosingScope
        {
            get
            {
                return enclosingScope;
            }
        }

        /// <summary>
        /// Return the name prefixed with the name of its enclosing scope
        ///  using '.' (dot) as the scope separator.
        /// </summary>
        public virtual string QualifiedName
        {
            get
            {
                return enclosingScope.Name + "." + Name;
            }
        }

        /// <summary>
        /// Return the name prefixed with the name of its enclosing scope. </summary>
        public virtual string getQualifiedName(string scopePathSeparator)
        {
            return enclosingScope.Name + scopePathSeparator + Name;
        }

        /// <summary>
        /// Return the fully qualified name includes all scopes from the root down
        ///  to this particular symbol.
        /// </summary>
        public virtual string getFullyQualifiedName(string scopePathSeparator)
        {
            IList<IScope> path = EnclosingPathToRoot;
            path.Reverse();
            return Utils.joinScopeNames(path, scopePathSeparator);
        }

        public virtual int InsertionOrderNumber
        {
            get
            {
                return index;
            }
            set
            {
                this.index = value;
            }
        }

        public override int NumberOfSymbols
        {
            get
            {
                return symbols.Count;
            }
        }

        public virtual int line { get { return Token != null ? Token.Line : 0; } }
        public virtual int col { get { return Token != null ? Token.Column : 0; } }
        public virtual string file { get { return Token != null ? Token.InputStream.SourceName : ""; } }
        public virtual IToken Token { get; protected set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ISymbol))
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }
            return Name.Equals(((ISymbol)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public ISymbol resolve()
        {
            return this;
        }
    }

}