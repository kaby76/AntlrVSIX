namespace org.antlr.symtab
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract base class that houses common functionality for
    ///  symbols like classes and functions that are both symbols and scopes.
    ///  There is some common cut and paste functionality with <seealso cref="BaseSymbol"/>
    ///  because of a lack of multiple inheritance in Java but it is minimal.
    /// </summary>
    public abstract class SymbolWithScope : BaseScope, Symbol, Scope
    {
        protected internal readonly string name; // All symbols at least have a name
        protected internal int index; // insertion order from 0; compilers often need this

        public SymbolWithScope(string name)
        {
            this.name = name;
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }
        public virtual Scope Scope
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
        public override Scope EnclosingScope
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
                return enclosingScope.Name + "." + name;
            }
        }

        /// <summary>
        /// Return the name prefixed with the name of its enclosing scope. </summary>
        public virtual string getQualifiedName(string scopePathSeparator)
        {
            return enclosingScope.Name + scopePathSeparator + name;
        }

        /// <summary>
        /// Return the fully qualified name includes all scopes from the root down
        ///  to this particular symbol.
        /// </summary>
        public virtual string getFullyQualifiedName(string scopePathSeparator)
        {
            IList<Scope> path = EnclosingPathToRoot;
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

        public override bool Equals(object obj)
        {
            if (!(obj is Symbol))
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }
            return name.Equals(((Symbol)obj).Name);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }

}