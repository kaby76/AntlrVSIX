namespace LanguageServer
{
    using Algorithms;
    using Antlr4.Runtime.Tree;
    using Microsoft.CodeAnalysis;
    using Symtab;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Workspaces;

    public class Module
    {

        public enum RecoveryStrategy { Bail, Standard };


        public int GetIndex(int line, int column, Document doc)
        {
            int index = 0;
            string buffer = doc.Code;
            if (buffer == null)
            {
                return 0;
            }

            int cur_line = 0;
            int cur_col = 0;
            for (; ; )
            {
                if (cur_line > line)
                {
                    break;
                }

                if (cur_line >= line && cur_col >= column)
                {
                    break;
                }

                char ch = buffer[index];
                if (ch == '\r')
                {
                    if (index + 1 >= buffer.Length)
                    {
                        break;
                    }
                    else if (buffer[index + 1] == '\n')
                    {
                        cur_line++;
                        cur_col = 0;
                        index += 2;
                    }
                    else
                    {
                        // Error in code.
                        cur_line++;
                        cur_col = 0;
                        index += 1;
                    }
                }
                else if (ch == '\n')
                {
                    cur_line++;
                    cur_col = 0;
                    index += 1;
                }
                else
                {
                    cur_col += 1;
                    index += 1;
                }
                if (index >= buffer.Length)
                {
                    break;
                }
            }
            return index;
        }

        public (int, int) GetLineColumn(int index, Document doc)
        {
            int cur_index = 0;
            string buffer = doc.Code;
            if (buffer == null)
            {
                return (0, 0);
            }

            int cur_line = 0; // zero based LSP.
            int cur_col = 0; // zero based LSP.
            for (; ; )
            {
                if (cur_index >= buffer.Length)
                {
                    break;
                }

                if (cur_index >= index)
                {
                    break;
                }

                char ch = buffer[cur_index];
                if (ch == '\r')
                {
                    if (cur_index + 1 >= buffer.Length)
                    {
                        break;
                    }
                    else if (buffer[cur_index + 1] == '\n')
                    {
                        cur_line++;
                        cur_col = 0;
                        cur_index += 2;
                    }
                    else
                    {
                        // Error in code.
                        cur_line++;
                        cur_col = 0;
                        cur_index += 1;
                    }
                }
                else if (ch == '\n')
                {
                    cur_line++;
                    cur_col = 0;
                    cur_index += 1;
                }
                else
                {
                    cur_col += 1;
                    cur_index += 1;
                }
                if (cur_index >= buffer.Length)
                {
                    break;
                }
            }
            return (cur_line, cur_col);
        }

        public QuickInfo GetQuickInfo(int index, Document doc)
        {
            ParsingResults pd = ParsingResultsFactory.Create(doc);
            if (pd.ParseTree == null)
            {
                Compile();
            }

            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            IParserDescription gd = ParserDescriptionFactory.Create(doc);
            if (pt == null)
            {
                return null;
            }

            Antlr4.Runtime.Tree.IParseTree p = pt;
            pd.Attributes.TryGetValue(p, out IList<CombinedScopeSymbol> list_value);
            if (list_value == null)
            {
                return null;
            }

            TerminalNodeImpl q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            Range range = new Workspaces.Range(new Workspaces.Index(q.Symbol.StartIndex), new Workspaces.Index(q.Symbol.StopIndex + 1));
            bool found = pd.PopupList.TryGetValue(q, out int tag_type);
            if (!found)
            {
                return null;
            }

            if (list_value == null || list_value.Count == 0)
            {
                return new QuickInfo() { Display = gd.Map[tag_type], Range = range };
            }
            if (list_value.Count == 1)
            {
                CombinedScopeSymbol value = list_value.First();
                Symtab.ISymbol name = value as Symtab.ISymbol;
                string show = name?.Name;
                if (value is Symtab.Literal)
                {
                    show = ((Symtab.Literal)value).Cleaned;
                }
                if (gd.PopUpDefinition[tag_type] != null)
                {
                    Func<ParsingResults, IParseTree, string> fun = gd.PopUpDefinition[tag_type];
                    string mess = fun(pd, p);
                    if (mess != null)
                    {
                        return new QuickInfo() { Display = mess, Range = range };
                    }
                }
                string display = gd.Map[tag_type]
                    + "\n"
                    + show;
                return new QuickInfo() { Display = display, Range = range };
            }
            {
                string display = "Ambiguous -- ";
                foreach (CombinedScopeSymbol value in list_value)
                {
                    Symtab.ISymbol name = value as Symtab.ISymbol;
                    string show = name?.Name;
                    if (value is Symtab.Literal)
                    {
                        show = ((Symtab.Literal)value).Cleaned;
                    }
                    if (gd.PopUpDefinition[tag_type] != null)
                    {
                        Func<ParsingResults, IParseTree, string> fun = gd.PopUpDefinition[tag_type];
                        string mess = fun(pd, p);
                        if (mess != null)
                        {
                            display = display + mess;
                        }
                    }
                    else
                    {
                        display = display + gd.Map[tag_type]
                            + "\n"
                            + show;
                    }
                }
                return new QuickInfo() { Display = display, Range = range };
            }
        }

        public int GetTag(int index, Document doc)
        {
            ParsingResults pd = ParsingResultsFactory.Create(doc);
            if (pd.ParseTree == null)
            {
                Compile();
            }

            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            IParserDescription gd = ParserDescriptionFactory.Create(doc);
            if (pt == null)
            {
                return -1;
            }

            Antlr4.Runtime.Tree.IParseTree p = pt;
            TerminalNodeImpl q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            bool found = pd.PopupList.TryGetValue(q, out int tag_type);
            if (found)
            {
                return tag_type;
            }

            if (q.Symbol == null)
            {
                return -1;
            }

            bool found2 = pd.Comments.TryGetValue(q.Symbol, out int tag2);
            if (found2)
            {
                return tag2;
            }

            return -1;
        }

        public DocumentSymbol GetDocumentSymbol(int index, Document doc)
        {
            ParsingResults pd = ParsingResultsFactory.Create(doc);
            if (pd.ParseTree == null)
            {
                Compile();
            }

            Antlr4.Runtime.Tree.IParseTree pt = LanguageServer.Util.Find(index, doc);
            IParserDescription gd = ParserDescriptionFactory.Create(doc);
            if (pt == null)
            {
                return default(DocumentSymbol);
            }

            Antlr4.Runtime.Tree.IParseTree p = pt;
            TerminalNodeImpl q = p as Antlr4.Runtime.Tree.TerminalNodeImpl;
            bool found = pd.PopupList.TryGetValue(q, out int tag_type);
            if (!found)
            {
                return null;
            }

            if (q.Symbol == null)
            {
                return null;
            }

            return new DocumentSymbol()
            {
                name = q.Symbol.Text,
                range = new Workspaces.Range(q.Symbol.StartIndex, q.Symbol.StopIndex),
                kind = tag_type
            };
        }

        public IEnumerable<DocumentSymbol> Get(Document doc)
        {
            ParsingResults pd = ParsingResultsFactory.Create(doc);
            if (pd.ParseTree == null)
            {
                Compile();
            }

            List<DocumentSymbol> combined = new List<DocumentSymbol>();
            foreach (KeyValuePair<TerminalNodeImpl, int> p in pd.Defs)
            {
                if (p.Key == null)
                {
                    continue;
                }

                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.GetText(),
                        range = new Workspaces.Range(p.Key.Payload.StartIndex, p.Key.Payload.StopIndex),
                        kind = p.Value
                    });
            }
            foreach (KeyValuePair<TerminalNodeImpl, int> p in pd.Refs)
            {
                if (p.Key == null)
                {
                    continue;
                }

                combined.Add(
                    new DocumentSymbol()
                    {
                        name = p.Key.GetText(),
                        range = new Workspaces.Range(p.Key.Payload.StartIndex, p.Key.Payload.StopIndex),
                        kind = p.Value
                    });
            }

            // Sort the list.
            IOrderedEnumerable<DocumentSymbol> sorted_combined_tokens = combined.OrderBy(t => t.range.Start.Value).ThenBy(t => t.range.End.Value);
            return sorted_combined_tokens;
        }

        public class Info
        {
            public int start;
            public int end;
            public int kind; 
        }

        public IEnumerable<Info> Get(int start, int end, Document doc)
        {
            try
            {
                ParsingResults pd = ParsingResultsFactory.Create(doc);
                if (pd.ParseTree == null)
                {
                    Compile();
                }

                List<Info> combined = new List<Info>();
                foreach (KeyValuePair<Antlr4.Runtime.IToken, int> p in pd.ColorizedList)
                {
                    if (p.Key == null)
                    {
                        continue;
                    }
                    var sym = p.Key;
                    var st = sym.StartIndex;
                    var en = sym.StopIndex + 1;
                    if (end < st) continue;
                    if (en < start) continue;
                    int s1 = st > start ? st : start;
                    int s2 = en < end ? en : end;
                    combined.Add(
                         new Info()
                         {
                             start = s1,
                             end = s2,
                             kind = p.Value
                         });
                    ;
                }
                // Sort the list.
                IOrderedEnumerable<Info> sorted_combined_tokens = combined.OrderBy(t => t.start).ThenBy(t => t.end);
                return sorted_combined_tokens;
            }
            catch (Exception eeks)
            {
            }
            return new List<Info>();
        }

        public IEnumerable<Workspaces.Range> GetErrors(Workspaces.Range range, Document doc)
        {
            ParsingResults pd = ParsingResultsFactory.Create(doc);
            if (pd.ParseTree == null)
            {
                Compile();
            }

            List<Range> result = new List<Workspaces.Range>();
            foreach (IParseTree p in pd.Errors)
            {
                ErrorNodeImpl q = p as Antlr4.Runtime.Tree.ErrorNodeImpl;
                if (q == null)
                {
                    continue;
                }

                if (q.Payload == null)
                {
                    continue;
                }

                int y = q.Payload.StartIndex;
                int z = q.Payload.StopIndex;
                if (y < 0)
                {
                    y = 0;
                }

                if (z < 0)
                {
                    z = 0;
                }

                int a = y;
                int b = z + 1;
                int start_token_start = a;
                int end_token_end = b;
                if (start_token_start > range.End.Value)
                {
                    continue;
                }

                if (end_token_end < range.Start.Value)
                {
                    continue;
                }

                Range r = new Workspaces.Range(new Workspaces.Index(a), new Workspaces.Index(b));
                result.Add(r);
            }
            return result;
        }

        public IList<Location> FindDefs(int index, Document doc)
        {
            List<Location> result = new List<Location>();
            if (doc == null)
            {
                return result;
            }

            IParseTree ref_pt = Util.Find(index, doc);
            if (ref_pt == null)
            {
                return result;
            }

            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            ref_pd.Attributes.TryGetValue(ref_pt, out IList<Symtab.CombinedScopeSymbol> list_values);
            if (list_values == null)
            {
                return result;
            }

            foreach (CombinedScopeSymbol value in list_values)
            {
                if (value == null)
                {
                    continue;
                }

                Symtab.ISymbol @ref = value as Symtab.ISymbol;
                if (@ref == null)
                {
                    continue;
                }

                List<Symtab.ISymbol> defs = @ref.resolve();
                if (defs == null)
                {
                    continue;
                }

                foreach (var def in defs)
                {
                    string def_file = def.file;
                    if (def_file == null)
                    {
                        continue;
                    }

                    Document def_item = Workspaces.Workspace.Instance.FindDocument(def_file);
                    if (def_item == null)
                    {
                        continue;
                    }

                    Location new_loc = new Location()
                    {
                        Range = new Workspaces.Range(def.Token.StartIndex, def.Token.StopIndex),
                        Uri = def_item
                    };
                    result.Add(new_loc);
                }
            }
            return result;
        }

        public IEnumerable<Location> FindRefsAndDefs(int index, Document doc)
        {
            List<Location> result = new List<Location>();
            IParseTree ref_pt = Util.Find(index, doc);
            if (ref_pt == null)
            {
                return result;
            }

            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            ref_pd.Attributes.TryGetValue(ref_pt, out IList<Symtab.CombinedScopeSymbol> list_value);
            if (list_value == null)
            {
                return result;
            }

            List<Symtab.ISymbol> found_defs = null;
            Symtab.ISymbol found_ref = null;
            foreach (CombinedScopeSymbol value in list_value)
            {
                if (value == null)
                {
                    continue;
                }

                Symtab.ISymbol @ref = value as Symtab.ISymbol;
                if (@ref == null)
                {
                    continue;
                }

                if (@ref.Token == null)
                {
                    continue;
                }

                found_ref = @ref;
                List<Symtab.ISymbol> defs = @ref.resolve();
                if (defs == null)
                {
                    continue;
                }
                found_defs = defs;
                break;
            }
            if (found_defs != null)
            {
                foreach (var def in found_defs)
                {
                    result.Add(
                        new Location()
                        {
                            Range = new Workspaces.Range(def.Token.StartIndex, def.Token.StopIndex),
                            Uri = Workspaces.Workspace.Instance.FindDocument(def.file)
                        });
                    var dd = def as BaseSymbol;
                    foreach (var r in dd.Refs)
                    {
                        result.Add(
                            new Location()
                            {
                                Range = new Workspaces.Range(r.Token.StartIndex, r.Token.StopIndex),
                                Uri = Workspaces.Workspace.Instance.FindDocument(r.file)
                            });
                    }
                }
            }
            return result;
        }

        public IEnumerable<Location> GetDefs(Document doc)
        {
            List<Location> result = new List<Location>();
            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            foreach (KeyValuePair<TerminalNodeImpl, int> value in ref_pd.Defs)
            {
                TerminalNodeImpl key = value.Key;
                Antlr4.Runtime.IToken sym = key.Payload;
                result.Add(
                    new Location()
                    {
                        Range = new Workspaces.Range(sym.StartIndex, sym.StopIndex),
                        Uri = Workspaces.Workspace.Instance.FindDocument(sym.InputStream.SourceName)
                    });
            }
            return result;
        }

        public IEnumerable<TerminalNodeImpl> GetRefsLeaf(Document doc)
        {
            List<TerminalNodeImpl> result = new List<TerminalNodeImpl>();
            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            foreach (KeyValuePair<TerminalNodeImpl, int> value in ref_pd.Refs)
            {
                TerminalNodeImpl key = value.Key;
                result.Add(key);
            }
            return result;
        }

        public IEnumerable<TerminalNodeImpl> GetDefsLeaf(Document doc)
        {
            List<TerminalNodeImpl> result = new List<TerminalNodeImpl>();
            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            foreach (KeyValuePair<TerminalNodeImpl, int> value in ref_pd.Defs)
            {
                TerminalNodeImpl key = value.Key;
                result.Add(key);
            }
            return result;
        }

        private Digraph<ParsingResults> ConstructGraph(IEnumerable<ParsingResults> to_do)
        {
            Digraph<ParsingResults> g = new Digraph<ParsingResults>();
            HashSet<ParsingResults> done = new HashSet<ParsingResults>();
            Stack<ParsingResults> stack = new Stack<ParsingResults>();
            foreach (ParsingResults f in to_do)
            {
                stack.Push(f);
            }

            while (stack.Count > 0)
            {
                ParsingResults f = stack.Pop();
                g.AddVertex(f);
                done.Add(f);
                foreach (string d in f.PropagateChangesTo)
                {
                    Document d_doc = Workspace.Instance.FindDocument(d);
                    ParsingResults d_pd = ParsingResultsFactory.Create(d_doc);
                    if (done.Contains(d_pd))
                    {
                        continue;
                    }

                    stack.Push(d_pd);
                }
            }

            foreach (ParsingResults v in g.Vertices)
            {
                HashSet<string> deps = v.PropagateChangesTo;
                Document doc = Workspace.Instance.FindDocument(v.FullFileName);
                ParsingResults pd = ParsingResultsFactory.Create(doc);
                foreach (string d in deps)
                {
                    Document d_doc = Workspace.Instance.FindDocument(d);
                    ParsingResults d_pd = ParsingResultsFactory.Create(d_doc);
                    g.AddEdge(new DirectedEdge<ParsingResults>(pd, d_pd));
                }
            }

            return g;
        }

        public List<ParsingResults> Compile(int quiet_after = 0)
        {
            try
            {
                Workspace ws = Workspaces.Workspace.Instance;

                // Get all changed files.
                HashSet<ParsingResults> to_do = new HashSet<ParsingResults>();

            DoAgain:

                // Get current directory, and add all grammar files.
                foreach (Document document in Workspaces.DFSContainer.DFS(ws))
                {
                    string file_name = document.FullPath;
                    if (file_name == null)
                    {
                        continue;
                    }

                    Container parent = document.Parent;
                    IParserDescription gd = LanguageServer.ParserDescriptionFactory.Create(document);
                    if (gd == null)
                    {
                        if (document.ParseAs != null)
                            System.Console.Error.WriteLine("Unknown parse type.");
                        continue;
                    }

                    // Get suffix of file_name.
                    string extension = System.IO.Path.GetExtension(file_name);
                    string directory = System.IO.Path.GetDirectoryName(file_name);
                    if (directory == "") directory = ".";
                    foreach (string file in System.IO.Directory.GetFiles(directory))
                    {
                        if (System.IO.Path.GetExtension(file) != extension)
                        {
                            continue;
                        }

                        Document x = Workspaces.Workspace.Instance.FindDocument(file);
                        if (x == null)
                        {
                            // Add document.
                            Container proj = parent;
                            Document new_doc = new Workspaces.Document(file);
                            proj.AddChild(new_doc);
                        }

                        IParserDescription g2 = LanguageServer.ParserDescriptionFactory.Create(document);
                        if (g2 == null)
                        {
                            continue;
                        }

                        ParsingResults p2 = ParsingResultsFactory.Create(document);
                        if (!p2.Changed)
                        {
                            continue;
                        }

                        to_do.Add(p2);
                    }
                }

                foreach (Document document in Workspaces.DFSContainer.DFS(ws))
                {
                    string file_name = document.FullPath;
                    if (file_name == null)
                    {
                        continue;
                    }

                    IParserDescription gd = LanguageServer.ParserDescriptionFactory.Create(document);
                    if (gd == null)
                    {
                        continue;
                    }
                    // file_name can be a URI, so this doesn't make sense.
                    //if (!System.IO.File.Exists(file_name)) continue;
                    ParsingResults pd = ParsingResultsFactory.Create(document);
                    if (!pd.Changed)
                    {
                        continue;
                    }

                    to_do.Add(pd);
                }
                Digraph<ParsingResults> g = ConstructGraph(to_do);
                foreach (ParsingResults v in g.Vertices)
                {
                    v.Item.Changed = true; // Force.
                    v.Parse();
                }
                bool changed = true;
                for (int pass = 0; changed; pass++)
                {
                    changed = false;
                    foreach (ParsingResults v in g.Vertices)
                    {
                        int number_of_passes = v.Passes.Count;
                        if (pass < number_of_passes)
                        {
                            try
                            {
                                bool reset = v.Pass(pass);
                                if (reset)
                                {
                                    goto DoAgain;
                                }
                            }
#pragma warning disable 0168
                            catch (Exception eeks) { }
#pragma warning restore 0168
                            changed = true;
                        }
                    }
                }
                foreach (ParsingResults v in g.Vertices)
                {
                    v.GatherRefsDefsAndOthers();
                }
                foreach (ParsingResults v in g.Vertices)
                {
                    v.GatherErrors();
                }
                return g.Vertices.ToList();
            }
            catch (Exception e)
            {
                Logger.Log.Notify(e.ToString());
            }
            return new List<ParsingResults>();
        }

        public TextEdit[] Reformat(Document doc)
        {
            ParsingResults ref_pd = ParsingResultsFactory.Create(doc);
            string code = doc.Code;
            string corpus_location = Options.Option.GetString("CorpusLocation");
            if (corpus_location == null)
            {
                TextEdit[] result = new TextEdit[] { };
                return result;
            }

            string ffn = doc.FullPath;
            if (ffn == null)
            {
                TextEdit[] result = new TextEdit[] { };
                return result;
            }
            IParserDescription grammar_description = LanguageServer.ParserDescriptionFactory.Create(doc);
            if (grammar_description == null)
            {
                TextEdit[] result = new TextEdit[] { };
                return result;
            }
            org.antlr.codebuff.Tool.unformatted_input = code;
            try
            {
                string result = org.antlr.codebuff.Tool.Main(
                    new object[]
                    {
                "-g", grammar_description.Name,
                "-lexer", grammar_description.Lexer,
                "-parser", grammar_description.Parser,
                "-rule", grammar_description.StartRule,
                "-files", grammar_description.FileExtension,
                "-corpus", corpus_location,
                "-inoutstring",
                ""
                    });
                List<TextEdit> edits = new List<TextEdit>();
                Diff_match_patch diff = new Diff_match_patch();
                List<Diff> diffs = diff.Diff_main(code, result);
                List<Patch> patch = diff.Patch_make(diffs);
                //patch.Reverse();

                // Start edit session.
                int times = 0;
                int delta = 0;
                foreach (Patch p in patch)
                {
                    times++;
                    int start = p.start1 - delta;

                    int offset = 0;
                    foreach (Diff ed in p.diffs)
                    {
                        if (ed.operation == Operation.EQUAL)
                        {
                            //// Let's verify that.
                            int len = ed.text.Length;
                            //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                            //  new Span(start + offset, len));
                            //var tt = tokenSpan.GetText();
                            //if (ed.text != tt)
                            //{ }
                            offset = offset + len;
                        }
                        else if (ed.operation == Operation.DELETE)
                        {
                            int len = ed.text.Length;
                            //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                            //  new Span(start + offset, len));
                            //var tt = tokenSpan.GetText();
                            //if (ed.text != tt)
                            //{ }
                            TextEdit edit = new TextEdit()
                            {
                                range = new Workspaces.Range(
                                    new Workspaces.Index(start + offset),
                                    new Workspaces.Index(start + offset + len)),
                                NewText = ""
                            };
                            offset = offset + len;
                            edits.Add(edit);
                        }
                        else if (ed.operation == Operation.INSERT)
                        {
                            int len = ed.text.Length;
                            TextEdit edit = new TextEdit()
                            {
                                range = new Workspaces.Range(
                                    new Workspaces.Index(start + offset),
                                    new Workspaces.Index(start + offset)),
                                NewText = ed.text
                            };
                            edits.Add(edit);
                        }
                    }
                    delta = delta + (p.length2 - p.length1);
                }
                return edits.ToArray();
            }
            catch (Exception)
            {
                TextEdit[] result = new TextEdit[] { };
                return result;
            }
        }

        public Dictionary<string, TextEdit[]> Rename(int index, string new_text, Document doc)
        {
            IEnumerable<Location> locations = FindRefsAndDefs(index, doc);
            Dictionary<string, TextEdit[]> result = new Dictionary<string, TextEdit[]>();
            IEnumerable<Document> documents = locations.Select(r => r.Uri).OrderBy(q => q).Distinct();
            foreach (Document f in documents)
            {
                string fn = f.FullPath;
                IOrderedEnumerable<Location> per_file_changes = locations.Where(z => z.Uri == f).OrderBy(q => q.Range.Start.Value);
                StringBuilder sb = new StringBuilder();
                int previous = 0;
                string code = f.Code;
                foreach (Location l in per_file_changes)
                {
                    Document d = l.Uri;
                    string xx = d.FullPath;
                    Range r = l.Range;
                    string pre = code.Substring(previous, r.Start.Value - previous);
                    sb.Append(pre);
                    sb.Append(new_text);
                    previous = r.End.Value + 1;
                }
                string rest = code.Substring(previous);
                sb.Append(rest);
                string new_code = sb.ToString();
                List<TextEdit> edits = new List<TextEdit>();
                Diff_match_patch diff = new Diff_match_patch();
                List<Diff> diffs = diff.Diff_main(code, new_code);
                List<Patch> patch = diff.Patch_make(diffs);
                int times = 0;
                int delta = 0;
                foreach (Patch p in patch)
                {
                    times++;
                    int start = p.start1 - delta;
                    int offset = 0;
                    foreach (Diff ed in p.diffs)
                    {
                        if (ed.operation == Operation.EQUAL)
                        {
                            //// Let's verify that.
                            int len = ed.text.Length;
                            //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                            //  new Span(start + offset, len));
                            //var tt = tokenSpan.GetText();
                            //if (ed.text != tt)
                            //{ }
                            offset = offset + len;
                        }
                        else if (ed.operation == Operation.DELETE)
                        {
                            int len = ed.text.Length;
                            //var tokenSpan = new SnapshotSpan(buffer.CurrentSnapshot,
                            //  new Span(start + offset, len));
                            //var tt = tokenSpan.GetText();
                            //if (ed.text != tt)
                            //{ }
                            TextEdit edit = new TextEdit()
                            {
                                range = new Workspaces.Range(
                                    new Workspaces.Index(start + offset),
                                    new Workspaces.Index(start + offset + len)),
                                NewText = ""
                            };
                            offset = offset + len;
                            edits.Add(edit);
                        }
                        else if (ed.operation == Operation.INSERT)
                        {
                            int len = ed.text.Length;
                            TextEdit edit = new TextEdit()
                            {
                                range = new Workspaces.Range(
                                    new Workspaces.Index(start + offset),
                                    new Workspaces.Index(start + offset)),
                                NewText = ed.text
                            };
                            edits.Add(edit);
                        }
                    }
                    delta = delta + (p.length2 - p.length1);
                }
                TextEdit[] e = edits.ToArray();
                result.Add(fn, e);
            }
            return result;
        }

        public List<string> Completion(int char_index, Document document)
        {
            ParsingResults ref_pd = ParsingResultsFactory.Create(document);
            if (ref_pd.ParseTree == null)
            {
                Compile();
            }

            List<string> result = ref_pd.Candidates(char_index);
            return result;
        }

        public string AntlrVersion()
        {
            Type parser_type = typeof(ANTLRv4Parser);
            System.CodeDom.Compiler.GeneratedCodeAttribute MyAttribute =
                (System.CodeDom.Compiler.GeneratedCodeAttribute)Attribute.GetCustomAttribute(parser_type, typeof(System.CodeDom.Compiler.GeneratedCodeAttribute));
            return MyAttribute?.Version;
        }
    }
}

