using Antlr4.Runtime.Tree;
using Symtab;
using System.Collections.Generic;

namespace AntlrVSIX.GrammarDescription.Antlr
{
    public class AntlrParserDetails : ParserDetails
    {
        static Dictionary<string, Scope> _scopes = new Dictionary<string, Scope>();
        static Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>> _attributes = new Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>>();
        static Scope _global_scope = new SymbolTable().GLOBALS;

        public AntlrParserDetails(Document item)
        {
            P2Listener = new Pass2Listener(this);
            P1Listener = new Pass1Listener(this);
            var dir = item.FullPath;
            dir = System.IO.Path.GetDirectoryName(dir);
            _scopes.TryGetValue(dir, out Scope value);
            if (value == null)
            {
                value = new LocalScope(_global_scope);
                _scopes[dir] = value;
            }
            this.RootScope = value;
            _attributes.TryGetValue(dir, out Dictionary<IParseTree, CombinedScopeSymbol> at);
            if (at == null)
            {
                at = new Dictionary<IParseTree, CombinedScopeSymbol>();
                _attributes[dir] = at;
            }
            this.Attributes = at;
        }
    }
}
