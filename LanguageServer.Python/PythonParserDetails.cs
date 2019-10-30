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
        static IScope _global_scope = new SymbolTable().GLOBALS;

        public PythonParserDetails(Workspaces.Document item)
            : base(item)
        {
            Passes.Add(() =>
            {
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
