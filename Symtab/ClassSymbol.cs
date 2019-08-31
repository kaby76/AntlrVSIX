namespace org.antlr.symtab
{
    using System.Collections.Generic;

    /// <summary>
    /// A symbol representing the class. It is a kind of data aggregate
    ///  that has much in common with a struct.
    /// </summary>
    public class ClassSymbol : DataAggregateSymbol
    {
        protected internal string superClassName; // null if this is Object
        protected internal int nextFreeMethodSlot = 0; // next slot to allocate

        public ClassSymbol(string name) : base(name)
        {
        }

        /// <summary>
        /// Return the ClassSymbol associated with superClassName or null if
        ///  superclass is not resolved looking up the enclosing scope chain.
        /// </summary>
        public virtual ClassSymbol SuperClassScope
        {
            get
            {
                if (!string.ReferenceEquals(superClassName, null))
                {
                    if (EnclosingScope != null)
                    {
                        Symbol superClass = EnclosingScope.resolve(superClassName);
                        if (superClass is ClassSymbol)
                        {
                            return (ClassSymbol)superClass;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Multiple superclass or interface implementations and the like... </summary>
        public virtual IList<ClassSymbol> SuperClassScopes
        {
            get
            {
                ClassSymbol superClassScope = SuperClassScope;
                if (superClassScope != null)
                {
                    IList<ClassSymbol> result = new List<ClassSymbol>();
                    result.Add(superClassScope);
                    return result;
                }
                return null;
            }
        }

        public override Symbol resolve(string name, bool alias = false)
        {
            Symbol s = resolveMember(name);
            if (s != null)
            {
                return s;
            }
            // if not a member, check any enclosing scope. it might be a global variable for example
            Scope parent = EnclosingScope;
            if (parent != null)
            {
                return parent.resolve(name, alias);
            }
            return null; // not found
        }

        /// <summary>
        /// Look for a member with this name in this scope or any super class.
        ///  Return null if no member found.
        /// </summary>
        public override Symbol resolveMember(string name)
        {
            Symbol s = symbols[name];
            if (s is MemberSymbol)
            {
                return s;
            }
            // walk superclass chain
            IList<ClassSymbol> superClassScopes = SuperClassScopes;
            if (superClassScopes != null)
            {
                foreach (ClassSymbol sup in superClassScopes)
                {
                    s = sup.resolveMember(name);
                    if (s is MemberSymbol)
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Look for a field with this name in this scope or any super class.
        ///  Return null if no field found.
        /// </summary>
        public override Symbol resolveField(string name)
        {
            Symbol s = resolveMember(name);
            if (s is FieldSymbol)
            {
                return s;
            }
            return null;
        }

        /// <summary>
        /// Look for a method with this name in this scope or any super class.
        ///  Return null if no method found.
        /// </summary>
        public virtual MethodSymbol resolveMethod(string name)
        {
            Symbol s = resolveMember(name);
            if (s is MethodSymbol)
            {
                return (MethodSymbol)s;
            }
            return null;
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

        public override void setSlotNumber(Symbol sym)
        {
            {
                if (sym is MethodSymbol)
                {
                    MethodSymbol msym = (MethodSymbol)sym;
                    // handle inheritance. If not found in this scope, check superclass
                    // if any.
                    ClassSymbol superClass = SuperClassScope;
                    if (superClass != null)
                    {
                        MethodSymbol superMethodSym = superClass.resolveMethod(sym.Name);
                        if (superMethodSym != null)
                        {
                            msym.slot = superMethodSym.slot;
                        }
                    }
                    if (msym.slot == -1)
                    {
                        msym.slot = nextFreeMethodSlot++;
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
                foreach (MemberSymbol s in Symbols)
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
                ClassSymbol superClassScope = SuperClassScope;
                if (superClassScope != null)
                {
                    methods.UnionWith(superClassScope.Methods);
                }
    //JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'removeAll' method:
                methods.Clear(); // override method from superclass
                methods.UnionWith(DefinedMethods);
                return methods;
            }
        }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.List<? extends FieldSymbol> getFields()
        public override IList<FieldSymbol> Fields
        {
            get
            {
                IList<FieldSymbol> fields = new List<FieldSymbol>();
                ClassSymbol superClassScope = SuperClassScope;
                if (superClassScope != null)
                {
                    ((List<FieldSymbol>)fields).AddRange(superClassScope.Fields);
                }
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
                foreach (MemberSymbol s in Symbols)
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
                int n = 0;
                ClassSymbol superClassScope = SuperClassScope;
                if (superClassScope != null)
                {
                    n += superClassScope.NumberOfMethods;
                }
                n += NumberOfDefinedMethods;
                return n;
            }
        }

        public override int NumberOfFields
        {
            get
            {
                int n = 0;
                ClassSymbol superClassScope = SuperClassScope;
                if (superClassScope != null)
                {
                    n += superClassScope.NumberOfFields;
                }
                n += NumberOfDefinedFields;
                return n;
            }
        }

        public override string ToString()
        {
            return name + ":" + base.ToString();
        }
    }

}