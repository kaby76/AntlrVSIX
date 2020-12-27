namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An abstract base class used to house common functionality.
    ///  You can associate a node in the parse tree that is responsible
    ///  for defining this symbol.
    /// </summary>
    public abstract class BaseSymbol : CombinedScopeSymbol, ISymbol
    {
        protected internal IType type; // If language statically typed, record type
        protected internal IScope scope; // All symbols know what scope contains them.
        protected internal ParserRuleContext defNode; // points at definition node in tree
        protected internal int lexicalOrder; // order seen or insertion order from 0; compilers often need this

        public virtual List<ISymbol> resolve()
        {
            // Look for definers in scope.
            List<ISymbol> res = this.scope.LookupType(this.Name).ToList();
            return res;
        }

        public BaseSymbol(string name, IList<IToken> token)
        {
            Name = name;
            Token = token;
        }

        public virtual string Name { get; set; }

        public virtual IScope Scope
        {
            get => scope;
            set => scope = value;
        }

        public virtual IType Type
        {
            get => type;
            set => type = value;
        }

        public virtual ParserRuleContext DefNode
        {
            set => defNode = value;
            get => defNode;
        }

        public HashSet<ISymbol> Refs { get; set; } = new HashSet<ISymbol>();

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

        public virtual int InsertionOrderNumber
        {
            get => lexicalOrder;
            set => lexicalOrder = value;
        }


        public virtual string getFullyQualifiedName(string scopePathSeparator)
        {
            IList<IScope> path = scope.EnclosingPathToRoot;
            path.Reverse();
            string qualifier = Utils.joinScopeNames(path, scopePathSeparator);
            return qualifier + scopePathSeparator + Name;
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
                    ts = ((SymbolWithScope)type).getFullyQualifiedName(".");
                }
                return '<' + s + Name + ":" + ts + '>';
            }
            return s + Name;
        }

        public ISymbol definition { get; set; }
        public virtual int line => Token != null && Token.Any() ? Token.First().Line : 0;
        public virtual int col => Token != null && Token.Any() ? Token.First().Column : 0;
        public virtual string file => Token != null && Token.Any() ? Token.First().InputStream.SourceName : "";
        public virtual IList<IToken> Token { get; protected set; }
    }

}