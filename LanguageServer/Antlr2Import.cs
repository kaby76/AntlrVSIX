namespace LanguageServer
{
    // See https://tomassetti.me/migrating-from-antlr2-to-antlr4/
    // https://theantlrguy.atlassian.net/wiki/spaces/ANTLR3/pages/2687070/Migrating+from+ANTLR+2+to+ANTLR+3
    // http://dust.ess.uci.edu/ppr/ppr_Par07.pdf

    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using org.eclipse.wst.xml.xpath2.processor.util;

    public class Antlr2Import
    {
        public static void Try(string ffn, string input, ref Dictionary<string, string> results)
        {
            var convert_undefined_to_terminals = true;
            var now = DateTime.Now.ToString();
            var errors = new StringBuilder();
            var str = new AntlrInputStream(input);
            var lexer = new ANTLRv2Lexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ANTLRv2Parser(tokens);
            var elistener = new ErrorListener<IToken>(parser, lexer, tokens);
            parser.AddErrorListener(elistener);
            var tree = parser.grammar_();
            var error_file_name = ffn;
            error_file_name = error_file_name.EndsWith(".g2")
                ? (error_file_name.Substring(0, error_file_name.Length - 3) + ".txt") : error_file_name;
            error_file_name = error_file_name.EndsWith(".g")
                ? (error_file_name.Substring(0, error_file_name.Length - 2) + ".txt") : error_file_name;

            var new_ffn = ffn;
            new_ffn = new_ffn.EndsWith(".g2")
                ? (new_ffn.Substring(0, new_ffn.Length - 3) + ".g4") : new_ffn;
            new_ffn = new_ffn.EndsWith(".g")
                ? (new_ffn.Substring(0, new_ffn.Length - 2) + ".g4") : new_ffn;

            if (elistener.had_error)
            {
                results.Add(error_file_name, errors.ToString());
                return;
            }
            else
            {
                errors.AppendLine("File " + ffn + " parsed successfully.");
                errors.AppendLine("Date: " + now);
            }

            // Transforms derived from two sources:
            // https://github.com/senseidb/sensei/pull/23
            var (text_before, other) = TreeEdits.TextToLeftOfLeaves(tokens, tree);

            // Remove "header".
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    AntlrTreeEditing.AntlrDOM.ConvertToDOM.Try(tree, parser);
                // Allow language, tokenVocab, TokenLabelType, superClass
                var nodes = engine.parseExpression(
                        @"//header_",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                TreeEdits.Delete(tree, (in IParseTree n, out bool c) =>
                {
                    c = true;
                    return nodes.Contains(n) ? n : null;
                });
            }


            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, tree, text_before);
            var new_code = sb.ToString();
            results.Add(new_ffn, new_code);
        }
    }
}
