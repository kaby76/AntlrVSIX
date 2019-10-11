namespace LanguageServer
{
    using Workspaces;

    public class QuickInfo
    {
        public static string Get(int index, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return null;
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            pd.Tags.TryGetValue(q, out int tag_type);
            if (value != null)
            {
                var name = value as Symtab.Symbol;
                string show = name?.Name;
                if (value is Symtab.Literal)
                {
                    show = ((Symtab.Literal)value).Cleaned;
                }
                if (gd.PopUpDefinition[tag_type] != null)
                {
                    var fun = gd.PopUpDefinition[tag_type];
                    var mess = fun(pd, p);
                    if (mess != null)
                        return mess;
                }
                return gd.Map[tag_type]
                    + "\n"
                    + show;
            }
            return gd.Map[tag_type];
        }
    }
}
