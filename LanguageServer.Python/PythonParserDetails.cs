using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.Python
{
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    public class PythonParserDetails : ParserDetails
    {
        static Dictionary<string, IScope> _scopes = new Dictionary<string, IScope>();
        static Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>> _attributes = new Dictionary<string, Dictionary<IParseTree, Symtab.CombinedScopeSymbol>>();
        static IScope _global_scope = new SymbolTable().GLOBALS;

        public PythonParserDetails(Workspaces.Document item)
            : base(item)
        {
            Passes.Add(() =>
            {
                var dir = item.FullPath;
                dir = System.IO.Path.GetDirectoryName(dir);
                _scopes.TryGetValue(dir, out IScope value);
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
                return false;
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass1Listener(this), ParseTree);
                return false;
            });
            Passes.Add(() =>
            {
                ParseTreeWalker.Default.Walk(new Pass2Listener(this), ParseTree);
                return false;
            });
        }
    }
}
