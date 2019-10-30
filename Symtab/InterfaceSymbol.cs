namespace Symtab
{
    using System.Collections.Generic;
    using Antlr4.Runtime;

    /// <summary>
    /// A symbol representing the class. It is a kind of data aggregate
    ///  that has much in common with a struct.
    /// </summary>
    public class InterfaceSymbol : DataAggregateSymbol
    {
        protected internal string superClassName; // null if this is Object
        protected internal int nextFreeMethodSlot = 0; // next slot to allocate

        public InterfaceSymbol(string n, IToken t) : base(n, t)
        {
        }

        /// <summary>
        /// Return the ClassSymbol associated with superClassName or null if
        ///  superclass is not resolved looking up the enclosing scope chain.
        /// </summary>
        public virtual IList<InterfaceSymbol> SuperClassScope
        {
            get
            {
                List<InterfaceSymbol> result = new List<InterfaceSymbol>();
                if (!string.ReferenceEquals(superClassName, null))
                {
                    if (EnclosingScope != null)
                    {
                        var list = EnclosingScope.LookupType(superClassName);
                        foreach (ISymbol superClass in list)
                            if (superClass is InterfaceSymbol)
                            {
                                result.Add( (InterfaceSymbol)superClass);
                            }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Multiple superclass or interface implementations and the like... </summary>
        public virtual IList<InterfaceSymbol> SuperClassScopes
        {
            get
            {
                return SuperClassScope;
            }
        }

        public override IList<ISymbol> LookupType(string name, bool alias = false)
        {
            var result = new List<ISymbol>();
            var list = resolveMember(name);
            result.AddRange(list);
            // if not a member, check any enclosing scope. it might be a global variable for example
            IScope parent = EnclosingScope;
            if (parent != null)
            {
                var another_list = parent.LookupType(name, alias);
                result.AddRange(another_list);
            }
            return result;
        }

        /// <summary>
        /// Look for a member with this name in this scope or any super class.
        ///  Return null if no member found.
        /// </summary>
        public override IList<ISymbol> resolveMember(string name)
        {
            var result = new List<ISymbol>();
            symbols.TryGetValue(name, out ISymbol s);
            if (s is IMemberSymbol)
            {
                result.Add(s);
            }
            // walk superclass chain
            IList<InterfaceSymbol> superClassScopes = SuperClassScopes;
            if (superClassScopes != null)
            {
                foreach (InterfaceSymbol sup in superClassScopes)
                {
                    var list = sup.resolveMember(name);
                    foreach (var ss in list)
                        if (ss is IMemberSymbol)
                        {
                            result.Add(ss);
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// Look for a field with this name in this scope or any super class.
        ///  Return null if no field found.
        /// </summary>
        public override IList<FieldSymbol> resolveField(string name)
        {
            var result = new List<FieldSymbol>();
            IList<ISymbol> list = resolveMember(name);
            foreach (var s in list)
                if (s is FieldSymbol)
                {
                    result.Add(s as FieldSymbol);
                }
            return result;
        }

        /// <summary>
        /// Look for a method with this name in this scope or any super class.
        ///  Return null if no method found.
        /// </summary>
        public virtual IList<MethodSymbol> resolveMethod(string name)
        {
            var result = new List<MethodSymbol>();
            IList<ISymbol> list = resolveMember(name);
            foreach (var s in list)
                if (s is MethodSymbol)
                {
                    result.Add(s as MethodSymbol);
                }
            return result;
        }

        public virtual string SuperClass
        {
            set
            {
                this.superClassName = value;
                nextFreeMethodSlot = NumberOfMethods;
            }
        }

        public virtual string SuperClassName
        {
            get
            {
                return superClassName;
            }
        }

        public override void setSlotNumber(ISymbol sym)
        {
            {
                if (sym is MethodSymbol)
                {
                    MethodSymbol msym = (MethodSymbol)sym;
                    // handle inheritance. If not found in this scope, check superclass
                    // if any.
                    var list = SuperClassScope;
                    if (list.Count == 1)
                    {
                        InterfaceSymbol superClass = list[0];
                        if (superClass != null)
                        {
                            var list_methods = superClass.resolveMethod(sym.Name);
                            if (list_methods.Count > 0)
                            {
                                MethodSymbol superMethodSym = list_methods[0];
                                if (superMethodSym != null)
                                {
                                    msym.slot = superMethodSym.slot;
                                }
                            }
                        }
                        if (msym.slot == -1)
                        {
                            msym.slot = nextFreeMethodSlot++;
                        }
                    }
                }
                else
                {
                    base.setSlotNumber(sym);
                }
            }
        }

        /// <summary>
        /// Return the set of all methods defined within this class </summary>
        public virtual ISet<MethodSymbol> DefinedMethods
        {
            get
            {
                ISet<MethodSymbol> methods = new LinkedHashSet<MethodSymbol>();
                foreach (IMemberSymbol s in Symbols)
                {
                    if (s is MethodSymbol)
                    {
                        methods.Add((MethodSymbol)s);
                    }
                }
                return methods;
            }
        }

        /// <summary>
        /// Return the set of all methods either inherited or not </summary>
        public virtual ISet<MethodSymbol> Methods
        {
            get
            {
                ISet<MethodSymbol> methods = new LinkedHashSet<MethodSymbol>();
                var list_supers = SuperClassScope;
                foreach (InterfaceSymbol superClassScope in list_supers)
                {
                    if (superClassScope != null)
                    {
                        methods.UnionWith(superClassScope.Methods);
                    }
                }
                throw new System.Exception("Not implemented.");
                methods.Clear(); // override method from superclass
                methods.UnionWith(DefinedMethods);
                return methods;
            }
        }

        public override IList<FieldSymbol> Fields
        {
            get
            {
                List<FieldSymbol> fields = new List<FieldSymbol>();
                var list_supers = SuperClassScope;
                foreach (InterfaceSymbol superClassScope in list_supers)
                {
                    if (superClassScope != null)
                    {
                        fields.AddRange(superClassScope.Fields);
                    }
                }
                throw new System.Exception("Not implemented.");
                ((List<FieldSymbol>)fields).AddRange(DefinedFields);
                return fields;
            }
        }

        /// <summary>
        /// get the number of methods defined specifically in this class </summary>
        public virtual int NumberOfDefinedMethods
        {
            get
            {
                int n = 0;
                foreach (IMemberSymbol s in Symbols)
                {
                    if (s is MethodSymbol)
                    {
                        n++;
                    }
                }
                return n;
            }
        }

        /// <summary>
        /// get the total number of methods visible to this class </summary>
        public virtual int NumberOfMethods
        {
            get
            {
                throw new System.Exception("Not implemented.");
                //int n = 0;
                //InterfaceSymbol superClassScope = SuperClassScope;
                //if (superClassScope != null)
                //{
                //    n += superClassScope.NumberOfMethods;
                //}
                //n += NumberOfDefinedMethods;
                //return n;
            }
        }

        public override int NumberOfFields
        {
            get
            {
                throw new System.Exception("Not implemented.");
                //int n = 0;
                //InterfaceSymbol superClassScope = SuperClassScope;
                //if (superClassScope != null)
                //{
                //    n += superClassScope.NumberOfFields;
                //}
                //n += NumberOfDefinedFields;
                //return n;
            }
        }

        public override string ToString()
        {
            return Name + ":" + base.ToString();
        }
    }

}