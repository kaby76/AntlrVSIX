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

        public SymbolWithScope(string name, IList<IToken> token)
        {
            Name = name;
            Token = token;
        }

        public override string Name { get; set; }

        public virtual IScope Scope
        {
            get => enclosingScope;
            set => EnclosingScope = value;
        }

        public override IScope EnclosingScope => enclosingScope;

        /// <summary>
        /// Return the name prefixed with the name of its enclosing scope
        ///  using '.' (dot) as the scope separator.
        /// </summary>
        public virtual string QualifiedName => enclosingScope.Name + "." + Name;

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
            get => index;
            set => index = value;
        }

        public override int NumberOfSymbols => symbols.Count;

        public virtual int line => Token != null && Token.Any() ? Token.First().Line : 0;
        public virtual int col => Token != null && Token.Any() ? Token.First().Column : 0;
        public virtual string file => Token != null && Token.Any() ? Token.First().InputStream.SourceName : "";
        public virtual IList<IToken> Token { get; protected set; }

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

        public List<ISymbol> resolve()
        {
            return new List<ISymbol>() { this };
        }
    }

}