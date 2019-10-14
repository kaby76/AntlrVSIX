namespace LanguageServer.Python
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class Pass1Listener : Python3ParserBaseListener
    {
        private PythonParserDetails _pd;

        public Pass1Listener(PythonParserDetails pd)
        {
            _pd = pd;
        }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                if (_pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value) && value is IScope)
                    return node;
            }
            return null;
        }

        public IScope GetScope(IParseTree node)
        {
            if (node == null)
                return null;
            _pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value);
            return value as IScope;
        }

        public Pass1Listener()
        {
        }
    }
}
