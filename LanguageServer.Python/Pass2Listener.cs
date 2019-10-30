namespace LanguageServer.Python
{
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Symtab;
    using System.Collections.Generic;

    class Pass2Listener : Python3ParserBaseListener
    {
        private PythonParserDetails _pd;

        public Pass2Listener(PythonParserDetails pd)
        {
            _pd = pd;
        }

        public IParseTree NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
                if (list != null)
                {
                    if (list.Count == 1 && list[0] is IScope)
                        return node;
                }
            }
            return null;
        }

        public IScope GetScope(IParseTree node)
        {
            if (node == null)
                return null;
            _pd.Attributes.TryGetValue(node, out IList<CombinedScopeSymbol> list);
            if (list != null)
            {
                if (list.Count == 1 && list[0] is IScope)
                    return list[0] as IScope;
            }
            return null;
        }
    }
}
