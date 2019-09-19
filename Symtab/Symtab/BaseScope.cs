namespace Symtab
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract base class that houses common functionality for scopes. </summary>
    public abstract class BaseScope : CombinedScopeSymbol, Scope
    {
        public abstract string Name {get;}
        protected internal Scope enclosingScope; // null if this scope is the root of the scope tree

        /// <summary>
        /// All symbols defined in this scope; can include classes, functions,
        ///  variables, or anything else that is a Symbol impl. It does NOT
        ///  include non-Symbol-based things like LocalScope. See nestedScopes.
        /// </summary>
        internal Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        /// <summary>
        /// All directly contained scopes, typically LocalScopes within a
        ///  LocalScope or a LocalScope within a FunctionSymbol. This does not
        ///  include SymbolWithScope objects.
        /// </summary>
        protected internal IList<Scope> nestedScopesNotSymbols = new List<Scope>();

        public BaseScope()
        {
        }

        public BaseScope(Scope enclosingScope)
        {
            EnclosingScope = enclosingScope;
        }

        public virtual IDictionary<string, Symbol> Members
        {
            get
            {
                return symbols;
            }
        }

        public virtual Symbol getSymbol(string name)
        {
            symbols.TryGetValue(name, out Symbol result);
            return result;
        }

        public virtual Scope EnclosingScope
        {
            set
            {
                this.enclosingScope = value;
            }
            get
            {
                return enclosingScope;
            }
        }

        public virtual IList<Scope> AllNestedScopedSymbols
        {
            get
            {
                IList<Scope> scopes = new List<Scope>();
                Utils.getAllNestedScopedSymbols(this, scopes);
                return scopes;
            }
        }

        public virtual IList<Scope> NestedScopedSymbols
        {
            get
            {
                IList<Symbol> scopes = Utils.filter(Symbols, s => s is Scope);
                return (IList<Scope>)scopes; // force it to cast
            }
        }

        public virtual IList<Scope> NestedScopes
        {
            get
            {
                List<Scope> all = new List<Scope>();
                all.AddRange(NestedScopedSymbols);
                all.AddRange(nestedScopesNotSymbols);
                return all;
            }
        }

        /// <summary>
        /// Add a nested scope to this scope; could also be a FunctionSymbol
        ///  if your language allows nested functions.
        /// </summary>
        public virtual void nest(Scope scope)
        {
            if (scope is SymbolWithScope)
            {
                throw new System.ArgumentException("Add SymbolWithScope instance " + scope.Name + " via define()");
            }
            nestedScopesNotSymbols.Add(scope);
        }

        public virtual Symbol LookupType(string name, bool alias = false)
        {
            if (!alias)
            {
                symbols.TryGetValue(name, out Symbol s);
                if (s != null)
                {
                    return s;
                }
                // if not here, check any enclosing scope
                Scope parent = EnclosingScope;
                if (parent != null)
                {
                    return parent.LookupType(name, alias);
                }
                return null; // not found
            }
            else
            {
                var list = symbols.Where(kvp =>
                {
                    var a = kvp.Value as TypeAlias;
                    if (a != null)
                    {
                        if (a.targetType.Name == name) return true;
                    }
                    return false;
                });
                if (list.Count() > 1) return null;
                if (list.Count() == 1) return list.First().Value;
                // if not here, check any enclosing scope
                Scope parent = EnclosingScope;
                if (parent != null)
                {
                    return parent.LookupType(name, alias);
                }
                return null; // not found
            }
        }

        public virtual void define(ref Symbol sym)
        {
            if (symbols.ContainsKey(sym.Name))
            {
                System.Type t = sym.GetType();
                var s = System.Activator.CreateInstance(t, new object[] { "_generated_" + new System.Random().Next() } );
                sym = s as Symbol;
                // throw new System.ArgumentException("duplicate symbol " + sym.Name);
            }
            sym.Scope = this;
            sym.InsertionOrderNumber = symbols.Count; // set to insertion position from 0
            symbols[sym.Name] = sym;
        }


        /// <summary>
        /// Walk up enclosingScope until we find topmost. Note this is
        ///  enclosing scope not necessarily parent. This will usually be
        ///  a global scope or something, depending on your scope tree.
        /// </summary>
        public virtual Scope OuterMostEnclosingScope
        {
            get
            {
                Scope s = this;
                while (s.EnclosingScope != null)
                {
                    s = s.EnclosingScope;
                }
                return s;
            }
        }

        /// <summary>
        /// Walk up enclosingScope until we find an object of a specific type.
        ///  E.g., if you want to get enclosing method, you would pass in
        ///  MethodSymbol.class, unless of course you have created a subclass for
        ///  your language implementation.
        /// </summary>
        public virtual MethodSymbol getEnclosingScopeOfType(Type type)
        {
            Scope s = this;
            while (s != null)
            {
                if (s.GetType() == type)
                {
                    return (MethodSymbol)s;
                }
                s = s.EnclosingScope;
            }
            return null;
        }

        public virtual IList<Scope> EnclosingPathToRoot
        {
            get
            {
                IList<Scope> scopes = new List<Scope>();
                Scope s = this;
                while (s != null)
                {
                    scopes.Add(s);
                    s = s.EnclosingScope;
                }
                return scopes;
            }
        }

        public virtual IList<Symbol> Symbols
        {
            get
            {
                Dictionary<string, Symbol>.ValueCollection list = symbols.Values;
                IList<Symbol> result = new List<Symbol>();
                foreach (var l in list)
                {
                    result.Add(l);
                }
                return result;
            }
        }

        public virtual IList<Symbol> AllSymbols
        {
            get
            {
                IList<Symbol> syms = new List<Symbol>();
                ((List<Symbol>)syms).AddRange(Symbols);
                foreach (Symbol s in symbols.Values)
                {
                    if (s is Scope)
                    {
                        Scope scope = (Scope)s;
                        ((List<Symbol>)syms).AddRange(scope.AllSymbols);
                    }
                }
                return syms;
            }
        }

        public virtual int NumberOfSymbols
        {
            get
            {
                return symbols.Count;
            }
        }

        public virtual ISet<string> SymbolNames
        {
            get
            {
                ISet<string> set = new HashSet<string>();
                foreach (var t in symbols)
                {
                    set.Add(t.Key);
                }
                return set;
            }
        }

        public override string ToString()
        {
            return symbols.Keys.ToString();
        }

        public virtual string toScopeStackString(string separator)
        {
            return Utils.toScopeStackString(this, separator);
        }

        public virtual string toQualifierString(string separator)
        {
            return Utils.toQualifierString(this, separator);
        }

        public virtual string toTestString()
        {
            return toTestString(", ", ".");
        }

        public virtual string toTestString(string separator, string scopePathSeparator)
        {
            IList<Symbol> allSymbols = this.AllSymbols;
            IList<string> syms = Utils.map(allSymbols, s => s.Scope.Name + scopePathSeparator + s.Name);
            return Utils.join(syms, separator);
        }
    }

}