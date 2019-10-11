namespace LanguageServer
{
    using Workspaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Tag
    {
        public static int Get(int index, Document doc)
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
            if (found) return default(DocumentSymbol);
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
    }
}
