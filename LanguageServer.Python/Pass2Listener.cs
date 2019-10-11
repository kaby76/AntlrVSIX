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

        public Scope NearestScope(IParseTree node)
        {
            for (; node != null; node = node.Parent)
            {
                if (_pd.Attributes.TryGetValue(node, out CombinedScopeSymbol value) && value is Scope)
                    return (Scope)value;
            }
            return null;
        }
    }
}
