namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    /// <summary>
    /// A symbol representing the class. It is a kind of data aggregate
    ///  that has much in common with a struct.
    /// </summary>
    public class InterfaceSymbol : DataAggregateSymbol
    {
        protected internal string superClassName; // null if this is Object
        protected internal int nextFreeMethodSlot = 0; // next slot to allocate

        public InterfaceSymbol(string n, IList<IToken> t) : base(n, t)
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
                        IList<ISymbol> list = EnclosingScope.LookupType(superClassName);
                        foreach (ISymbol superClass in list)
                        {
                            if (superClass is InterfaceSymbol)
                            {
                                result.Add((InterfaceSymbol)superClass);
                            }
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Multiple superclass or interface implementations and the like... </summary>
        public virtual IList<InterfaceSymbol> SuperClassScopes => SuperClassScope;

        public override IList<ISymbol> LookupType(string name, bool alias = false)
        {
            List<ISymbol> result = new List<ISymbol>();
            IList<ISymbol> list = resolveMember(name);
            result.AddRange(list);
            // if not a member, check any enclosing scope. it might be a global variable for example
            IScope parent = EnclosingScope;
            if (parent != null)
            {
                IList<ISymbol> another_list = parent.LookupType(name, alias);
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
            List<ISymbol> result = new List<ISymbol>();
            symbols.TryGetValue(name, out List<ISymbol> ss);
            foreach (var s in ss)
            {
                if (s is IMemberSymbol)
                {
                    result.Add(s);
                }
            }
            // walk superclass chain
            IList<InterfaceSymbol> superClassScopes = SuperClassScopes;
            if (superClassScopes != null)
            {
                foreach (InterfaceSymbol sup in superClassScopes)
                {
                    IList<ISymbol> list = sup.resolveMember(name);
                    foreach (ISymbol ss2 in list)
                    {
                        if (ss2 is IMemberSymbol)
                        {
                            result.Add(ss2);
                        }
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
            List<FieldSymbol> result = new List<FieldSymbol>();
            IList<ISymbol> list = resolveMember(name);
            foreach (ISymbol s in list)
            {
                if (s is FieldSymbol)
                {
                    result.Add(s as FieldSymbol);
                }
            }

            return result;
        }

        /// <summary>
        /// Look for a method with this name in this scope or any super class.
        ///  Return null if no method found.
        /// </summary>
        public virtual IList<MethodSymbol> resolveMethod(string name)
        {
            List<MethodSymbol> result = new List<MethodSymbol>();
            IList<ISymbol> list = resolveMember(name);
            foreach (ISymbol s in list)
            {
                if (s is MethodSymbol)
                {
                    result.Add(s as MethodSymbol);
                }
            }

            return result;
        }

        public virtual string SuperClass
        {
            set
            {
                superClassName = value;
                nextFreeMethodSlot = NumberOfMethods;
            }
        }

        public virtual string SuperClassName => superClassName;

        public override void setSlotNumber(ISymbol sym)
        {
            {
                if (sym is MethodSymbol)
                {
                    MethodSymbol msym = (MethodSymbol)sym;
                    // handle inheritance. If not found in this scope, check superclass
                    // if any.
                    IList<InterfaceSymbol> list = SuperClassScope;
                    if (list.Count == 1)
                    {
                        InterfaceSymbol superClass = list[0];
                        if (superClass != null)
                        {
                            IList<MethodSymbol> list_methods = superClass.resolveMethod(sym.Name);
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
                IList<InterfaceSymbol> list_supers = SuperClassScope;
                foreach (InterfaceSymbol superClassScope in list_supers)
                {
                    if (superClassScope != null)
                    {
                        methods.UnionWith(superClassScope.Methods);
                    }
                }
                throw new System.Exception("Not implemented.");
                //methods.Clear(); // override method from superclass
                //methods.UnionWith(DefinedMethods);
                //return methods;
            }
        }

        public override IList<FieldSymbol> Fields
        {
            get
            {
                List<FieldSymbol> fields = new List<FieldSymbol>();
                IList<InterfaceSymbol> list_supers = SuperClassScope;
                foreach (InterfaceSymbol superClassScope in list_supers)
                {
                    if (superClassScope != null)
                    {
                        fields.AddRange(superClassScope.Fields);
                    }
                }
                throw new System.Exception("Not implemented.");
                //fields.AddRange(DefinedFields);
                //return fields;
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
        public virtual int NumberOfMethods => throw new System.Exception("Not implemented.");//int n = 0;//InterfaceSymbol superClassScope = SuperClassScope;//if (superClassScope != null)//{//    n += superClassScope.NumberOfMethods;//}//n += NumberOfDefinedMethods;//return n;

        public override int NumberOfFields => throw new System.Exception("Not implemented.");//int n = 0;//InterfaceSymbol superClassScope = SuperClassScope;//if (superClassScope != null)//{//    n += superClassScope.NumberOfFields;//}//n += NumberOfDefinedFields;//return n;

        public override string ToString()
        {
            return Name + ":" + base.ToString();
        }
    }

}