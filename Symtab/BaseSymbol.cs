namespace Symtab
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;

    /// <summary>
    /// An abstract base class used to house common functionality.
    ///  You can associate a node in the parse tree that is responsible
    ///  for defining this symbol.
    /// </summary>
    public abstract class BaseSymbol : CombinedScopeSymbol, Symbol
    {
        protected internal readonly string name; // All symbols at least have a name
        protected internal Type type; // If language statically typed, record type
        protected internal Scope scope; // All symbols know what scope contains them.
        protected internal ParserRuleContext defNode; // points at definition node in tree
        protected internal int lexicalOrder; // order seen or insertion order from 0; compilers often need this

        public virtual Symbol resolve()
        {
            return this;
        }

        public BaseSymbol(string name, int l, int c, string f)
        {
            this.name = name;
            this.line = l;
            this.col = c;
            this.file = f;
        }

        public virtual string Name
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
                return scope;
            }
            set
            {
                this.scope = value;
            }
        }

        public virtual Type Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
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

        public virtual int InsertionOrderNumber
        {
            get
            {
                return lexicalOrder;
            }
            set
            {
                this.lexicalOrder = value;
            }
        }


        public virtual string getFullyQualifiedName(string scopePathSeparator)
        {
            IList<Scope> path = scope.EnclosingPathToRoot;
            path.Reverse();
            string qualifier = Utils.joinScopeNames(path, scopePathSeparator);
            return qualifier + scopePathSeparator + name;
        }

        public override string ToString()
        {
            string s = "";
            if (scope != null)
            {
                s = scope.Name + ".";
            }
            if (type != null)
            {
                string ts = type.ToString();
                if (type is SymbolWithScope)
                {
                    ts = ((SymbolWithScope) type).getFullyQualifiedName(".");
                }
                return '<' + s + Name + ":" + ts + '>';
            }
            return s + Name;
        }

        public Symbol definition { get; set; }
        public virtual int line { get; set; }
        public virtual int col { get; set; }
        public virtual string file { get; set; }
    }

}