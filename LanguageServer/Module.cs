namespace LanguageServer
{
    using Workspaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Graphs;

    public class Module
    {
        public static string GetQuickInfo(int index, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            if (pd.ParseTree == null) return null;
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return null;
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            pd.Tags.TryGetValue(q, out int tag_type);
            if (value != null)
            {
                var name = value as Symtab.ISymbol;
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
            if (pd.ParseTree == null) return -1;
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return -1;
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            var found = pd.Tags.TryGetValue(q, out int tag_type);
            if (found) return tag_type;
            if (q.Symbol == null) return -1;
            var found2 = pd.Comments.TryGetValue(q.Symbol, out int tag2);
            if (found2) return tag2;
            return -1;
        }

        public static DocumentSymbol GetDocumentSymbol(int index, Document doc)
        {
            var pd = ParserDetailsFactory.Create(doc);
            if (pd.ParseTree == null) return null;
            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            var gd = GrammarDescriptionFactory.Create(doc.FullPath);
            if (pt == null) return default(DocumentSymbol);
            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out Symtab.CombinedScopeSymbol value);
            var q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            var found = pd.Tags.TryGetValue(q, out int tag_type);
            if (!found) return null;
            if (q.Symbol == null) return null;
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
            if (pd.ParseTree == null) return new List<DocumentSymbol>();
            var combined = new List<DocumentSymbol>();
            foreach (var p in pd.Tags)
            {
                if (p.Key.Symbol == null) continue;
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
            if (pd.ParseTree == null) return new List<DocumentSymbol>();
            var combined = new System.Collections.Generic.List<DocumentSymbol>();
            foreach (var p in pd.Tags)
            {
                if (p.Key.Symbol == null) continue;
                int start_token_start = p.Key.Symbol.StartIndex;
                int end_token_end = p.Key.Symbol.StopIndex + 1;
                if (start_token_start > range.End.Value) continue;
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
                int end_token_end = p.Key.StopIndex + 1;
                if (start_token_start > range.End.Value) continue;
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
            if (doc == null) return null;
            var ref_pt = Util.Find(index, doc);
            if (ref_pt == null) return default(Location);
            var ref_pd = ParserDetailsFactory.Create(doc);
            ref_pd.Attributes.TryGetValue(ref_pt, out Symtab.CombinedScopeSymbol value);
            if (value == null) return default(Location);
            var @ref = value as Symtab.ISymbol;
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
            var @ref = value as Symtab.ISymbol;
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
                    var vv = v as Symtab.ISymbol;
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

        private static Digraph<ParserDetails> ConstructGraph(List<ParserDetails> to_do)
        {
            Digraph<ParserDetails> g = new Digraph<ParserDetails>();
            HashSet<ParserDetails> done = new HashSet<ParserDetails>();
            Stack<ParserDetails> stack = new Stack<ParserDetails>();
            foreach (var f in to_do)
            {
                stack.Push(f);
            }

            while (stack.Count > 0)
            {
                var f = stack.Pop();
                g.AddVertex(f);
                done.Add(f);
                foreach (var d in f.PropagateChangesTo)
                {
                    var d_doc = Workspace.Instance.FindDocument(d);
                    var d_pd = ParserDetailsFactory.Create(d_doc);
                    if (done.Contains(d_pd)) continue;
                    stack.Push(d_pd);
                }
            }

            foreach (var v in g.Vertices)
            {
                var deps = v.PropagateChangesTo;
                var doc = Workspace.Instance.FindDocument(v.FullFileName);
                var pd = ParserDetailsFactory.Create(doc);
                foreach (var d in deps)
                {
                    var d_doc = Workspace.Instance.FindDocument(d);
                    var d_pd = ParserDetailsFactory.Create(d_doc);
                    g.AddEdge(new DirectedEdge<ParserDetails>(pd, d_pd));
                }
            }

            return g;
        }

        public static List<ParserDetails> Compile()
        {
            var ws = Workspaces.Workspace.Instance;

            // Get all changed files.
            List<ParserDetails> to_do = new List<ParserDetails>();
            foreach (var document in Workspaces.DFSContainer.DFS(ws))
            {
                string file_name = document.FullPath;
                if (file_name == null) continue;
                var gd = LanguageServer.GrammarDescriptionFactory.Create(file_name);
                if (gd == null) continue;
                if (!System.IO.File.Exists(file_name)) continue;
                var item = Workspaces.Workspace.Instance.FindDocument(file_name);
                var pd = ParserDetailsFactory.Create(item);
                if (!pd.Changed) continue;
                to_do.Add(pd);
            }

            Digraph<ParserDetails> g = ConstructGraph(to_do);
            foreach (var v in g.Vertices)
            {
                v.Item.Changed = true; // Force.
                v.Parse();
            }
            foreach (var v in g.Vertices)
            {
                v.Pass1();
            }
            foreach (var v in g.Vertices)
            {
                v.Pass2();
            }
            foreach (var v in g.Vertices)
            {
                v.GatherDefs();
            }
            foreach (var v in g.Vertices)
            {
                v.GatherRefs();
            }
            return g.Vertices.ToList();
        }
    }
}
