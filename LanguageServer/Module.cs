namespace LanguageServer
{
    using Workspaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Module
    {
        public static string GetQuickInfo(int index, Document doc)
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

        public static int GetTag(int index, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return -1;
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            var found = pd.Tags.TryGetValue(q, out int tag_type);
            if (found) return tag_type;
            var found2 = pd.Comments.TryGetValue(q.Symbol, out int tag2);
            if (found2) return tag2;
            return -1;
        }

        public static DocumentSymbol GetDocumentSymbol(int index, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return default(DocumentSymbol);
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            var found = pd.Tags.TryGetValue(q, out int tag_type);
            if (!found) return default(DocumentSymbol);
            return new DocumentSymbol()
            {
                name = q.Symbol.Text,
                range = new Range(q.Symbol.StartIndex, q.Symbol.StopIndex),
                kind = tag_type
            };
        }

        public static IEnumerable<DocumentSymbol> Get(Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            var combined = new System.Collections.Generic.List<DocumentSymbol>();
            foreach (var p in pd.Tags)
            {
                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.Symbol.Text,
                        range = new Range(p.Key.Symbol.StartIndex, p.Key.Symbol.StopIndex),
                        kind = p.Value
                    });
            }
            foreach (var p in pd.Comments)
            {
                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.Text,
                        range = new Range(p.Key.StartIndex, p.Key.StopIndex),
                        kind = p.Value
                    });
            }

            // Sort the list.
            var sorted_combined_tokens = combined.OrderBy((t) => t.range.Start);
            return sorted_combined_tokens;
        }

        public static IEnumerable<DocumentSymbol> Get(Range range, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            var combined = new System.Collections.Generic.List<DocumentSymbol>();
            foreach (var p in pd.Tags)
            {
                int start_token_start = p.Key.Symbol.StartIndex;
                int end_token_end = p.Key.Symbol.StopIndex;
                if (start_token_start >= range.End.Value) continue;
                if (end_token_end < range.Start.Value) continue;
                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.Symbol.Text,
                        range = new Range(p.Key.Symbol.StartIndex, p.Key.Symbol.StopIndex),
                        kind = p.Value
                    });
            }
            foreach (var p in pd.Comments)
            {
                int start_token_start = p.Key.StartIndex;
                int end_token_end = p.Key.StopIndex;
                if (start_token_start >= range.End.Value) continue;
                if (end_token_end < range.Start.Value) continue;
                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.Text,
                        range = new Range(p.Key.StartIndex, p.Key.StopIndex),
                        kind = p.Value
                    });
            }

            // Sort the list.
            IEnumerable<DocumentSymbol> result;
            var sorted_combined_tokens = combined.OrderBy(t => t.range.Start.Value).ThenBy(t => t.range.End.Value);
            result = sorted_combined_tokens;
            return result;
        }

        public static Location FindDef(int index, Document doc)
        {
            var ref_pt = Util.Find(index, doc);
            if (ref_pt == null) return default(Location);
            var ref_pd = ParserDetailsFactory.Create(doc);
            ref_pd.Attributes.TryGetValue(ref_pt, out Symtab.CombinedScopeSymbol value);
            if (value == null) return default(Location);
            var @ref = value as Symtab.Symbol;
            if (@ref == null) return default(Location);
            var def = @ref.resolve();
            if (def == null) return default(Location);
            var def_file = def.file;
            if (def_file == null) return default(Location);
            var def_item = Workspaces.Workspace.Instance.FindDocument(def_file);
            if (def_item == null) return default(Location);
            return new Location()
            {
                range = new Range(def.Token.StartIndex, def.Token.StopIndex),
                uri = def_item
            };

        }

        public static IEnumerable<Location> FindRefsAndDefs(int index, Document doc)
        {
            var result = new List<Location>();
            var ref_pt = Util.Find(index, doc);
            if (ref_pt == null) return result;
            var ref_pd = ParserDetailsFactory.Create(doc);
            ref_pd.Attributes.TryGetValue(ref_pt, out Symtab.CombinedScopeSymbol value);
            if (value == null) return result;
            var @ref = value as Symtab.Symbol;
            if (@ref == null) return result;
            if (@ref.Token == null) return result;
            var def = @ref.resolve();
            if (def == null) return result;
            if (def.Token == null) return result;
            List<Antlr4.Runtime.Tree.TerminalNodeImpl> where = new List<Antlr4.Runtime.Tree.TerminalNodeImpl>();
            var refs = ref_pd.Refs.Where(
                (t) =>
                {
                    Antlr4.Runtime.Tree.TerminalNodeImpl x = t.Key;
                    if (x == @ref.Token) return true;
                    ref_pd.Attributes.TryGetValue(x, out Symtab.CombinedScopeSymbol v);
                    var vv = v as Symtab.Symbol;
                    if (vv == null) return false;
                    if (vv.resolve() == def) return true;
                    return false;
                }).Select(t => t.Key);
            if (def != null)
            {
                if (def.file == @ref.file)
                     result.Add(
                        new Location()
                        {
                            range = new Range(def.Token.StartIndex, def.Token.StopIndex),
                            uri = Workspaces.Workspace.Instance.FindDocument(def.file)
                        });
            }
            foreach (var r in refs)
            {
                result.Add(
                        new Location()
                        {
                            range = new Range(r.Symbol.StartIndex, r.Symbol.StopIndex),
                            uri = Workspaces.Workspace.Instance.FindDocument(r.Symbol.InputStream.SourceName)
                        });
            }
            return result;
        }
    }
}
