namespace Symtab
{
    using Algorithms.Utils;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract base class that houses common functionality for scopes. </summary>
    public abstract class BaseScope : CombinedScopeSymbol, IScope
    {
        public virtual string Name { get; set; }

        protected internal IScope enclosingScope; // null if this scope is the root of the scope tree

        /// <summary>
        /// All symbols defined in this scope; can include classes, functions,
        ///  variables, or anything else that is a Symbol impl. It does NOT
        ///  include non-Symbol-based things like LocalScope. See nestedScopes.
        /// </summary>
        internal MultiMap<string, ISymbol> symbols = new MultiMap<string, ISymbol>();

        /// <summary>
        /// All directly contained scopes, typically LocalScopes within a
        ///  LocalScope or a LocalScope within a FunctionSymbol. This does not
        ///  include SymbolWithScope objects.
        /// </summary>
        protected internal IList<IScope> nestedScopesNotSymbols = new List<IScope>();

        public BaseScope() { }

        public BaseScope(IScope enclosingScope)
        {
            EnclosingScope = enclosingScope;
        }

        public virtual MultiMap<string, ISymbol> Members => symbols;

        public virtual List<ISymbol> getSymbol(string name)
        {
            symbols.TryGetValue(name, out List<ISymbol> result);
            return result;
        }

        public virtual IScope EnclosingScope
        {
            set => enclosingScope = value;
            get => enclosingScope;
        }

        public virtual IList<IScope> AllNestedScopedSymbols
        {
            get
            {
                IList<IScope> scopes = new List<IScope>();
                Utils.getAllNestedScopedSymbols(this, scopes);
                return scopes;
            }
        }

        public virtual IList<IScope> NestedScopedSymbols
        {
            get
            {
                List<IScope> result = new List<IScope>();
                foreach (ISymbol sym in Symbols)
                {
                    IScope s = sym as IScope;
                    if (s == null)
                    {
                        continue;
                    }

                    result.Add(s);
                }
                return result;
            }
        }

        public virtual IList<IScope> NestedScopes
        {
            get
            {
                List<IScope> all = new List<IScope>();
                all.AddRange(NestedScopedSymbols);
                all.AddRange(nestedScopesNotSymbols);
                return all;
            }
        }

        /// <summary>
        /// Add a nested scope to this scope; could also be a FunctionSymbol
        ///  if your language allows nested functions.
        /// </summary>
        public virtual void nest(IScope scope)
        {
            if (scope is SymbolWithScope)
            {
                throw new System.ArgumentException("Add SymbolWithScope instance " + scope.Name + " via define()");
            }
            nestedScopesNotSymbols.Add(scope);
        }

        public virtual IList<ISymbol> LookupType(string name, bool alias = false)
        {
            List<ISymbol> result = new List<ISymbol>();
            if (!alias)
            {
                symbols.TryGetValue(name, out List<ISymbol> ss);
                if (ss != null)
                {
                    foreach (var s in ss)
                    {
                        result.Add(s);
                    }
                }
                // if not here, check any enclosing scope
                IScope parent = EnclosingScope;
                if (parent != null)
                {
                    IList<ISymbol> possibles = parent.LookupType(name, alias);
                    if (possibles.Any())
                    {
                        result.AddRange(possibles);
                    }
                }
                // If not here, check enclosing "import" scopes.
                foreach (IScope sc in nestedScopesNotSymbols)
                {
                    if (sc is SearchPathScope)
                    {
                        SearchPathScope spsc = sc as SearchPathScope;
                        foreach (IScope sub_scope in spsc.nestedScopesNotSymbols)
                        {
                            IList<ISymbol> res = sub_scope.LookupType(name);
                            if (res != null)
                            {
                                if (res.Any())
                                {
                                    result.AddRange(res);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var list = symbols.Where(kvp =>
                {
                    List<ISymbol> kvpv = kvp.Value;
                    foreach (var x in kvpv)
                    {
                        TypeAlias a = x as TypeAlias;
                        if (a != null)
                        {
                            if (a.targetType.Name == name)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                });
                if (list.Any())
                {
                    var zs = list.Select(l => l.Value).ToList();
                    foreach (var z in zs)
                    {
                        result.AddRange(z);
                    }
                }
                // if not here, check any enclosing scope
                IScope parent = EnclosingScope;
                if (parent != null)
                {
                    IList<ISymbol> possibles = parent.LookupType(name, alias);
                    if (possibles.Any())
                    {
                        result.AddRange(possibles);
                    }
                }
                // If not here, check enclosing "import" scopes.
                foreach (IScope sc in nestedScopesNotSymbols)
                {
                    if (sc is SearchPathScope)
                    {
                        SearchPathScope spsc = sc as SearchPathScope;
                        foreach (IScope sub_scope in spsc.nestedScopesNotSymbols)
                        {
                            IList<ISymbol> res = sub_scope.LookupType(name);
                            if (res != null)
                            {
                                if (res.Any())
                                {
                                    result.AddRange(res);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public virtual void define(ref ISymbol sym)
        {
            sym.Scope = this;
            sym.InsertionOrderNumber = symbols.Count; // set to insertion position from 0
            symbols.Add(sym.Name, sym);
        }

        public virtual void remove(ISymbol sym)
        {
            if (symbols.ContainsKey(sym.Name))
            {
                symbols.Remove(sym.Name);
            }
            sym.Scope = null;
            sym.InsertionOrderNumber = -1;
        }

        public void empty()
        {
            symbols.Clear();
        }

        /// <summary>
        /// Walk up enclosingScope until we find topmost. Note this is
        ///  enclosing scope not necessarily parent. This will usually be
        ///  a global scope or something, depending on your scope tree.
        /// </summary>
        public virtual IScope OuterMostEnclosingScope
        {
            get
            {
                IScope s = this;
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
        public virtual MethodSymbol getEnclosingScopeOfType(IType type)
        {
            IScope s = this;
            while (s != null)
            {
                if (s.GetType() == type.GetType())
                {
                    return (MethodSymbol)s;
                }
                s = s.EnclosingScope;
            }
            return null;
        }

        public virtual IList<IScope> EnclosingPathToRoot
        {
            get
            {
                IList<IScope> scopes = new List<IScope>();
                IScope s = this;
                while (s != null)
                {
                    scopes.Add(s);
                    s = s.EnclosingScope;
                }
                return scopes;
            }
        }

        public virtual IList<ISymbol> Symbols
        {
            get
            {
                IEnumerable<ISymbol> list = symbols.Values;
                IList<ISymbol> result = new List<ISymbol>();
                foreach (ISymbol l in list)
                {
                    result.Add(l);
                }
                return result;
            }
        }

        public virtual IList<ISymbol> AllSymbols
        {
            get
            {
                IList<ISymbol> syms = new List<ISymbol>();
                ((List<ISymbol>)syms).AddRange(Symbols);
                foreach (ISymbol s in symbols.Values)
                {
                    if (s is IScope)
                    {
                        IScope scope = (IScope)s;
                        ((List<ISymbol>)syms).AddRange(scope.AllSymbols);
                    }
                }
                return syms;
            }
        }

        public virtual int NumberOfSymbols => symbols.Count;

        public virtual ISet<string> SymbolNames
        {
            get
            {
                ISet<string> set = new HashSet<string>();
                foreach (KeyValuePair<string, List<ISymbol>> t in symbols)
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
            IList<ISymbol> allSymbols = AllSymbols;
            IList<string> syms = Utils.map(allSymbols, s => s.Scope.Name + scopePathSeparator + s.Name);
            return Utils.join(syms, separator);
        }
    }

}