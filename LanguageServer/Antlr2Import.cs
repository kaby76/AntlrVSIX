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
        public bool StripLabelOperator { get; set; } = true; // "x:foobar" => "foobar"
        public bool StripActionBlocks { get; set; } = true; // "{ ... }" => ""
        public bool StripAssignmentOperator { get; set; } = true; // "x=foobar" => "foobar"

        public Antlr2Import()
        {
        }

        public void Try(string ffn, string input, ref Dictionary<string, string> results)
        {
            var now = DateTime.Now.ToString();
            var errors = new StringBuilder();
            var str = new AntlrInputStream(input);
            var lexer = new ANTLRv2Lexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ANTLRv2Parser(tokens);
            var elistener = new ErrorListener<IToken>(parser, lexer, 0);
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
            using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();               
                var nodes = engine.parseExpression(
                        @"//header_",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                foreach (var n in nodes) TreeEdits.Delete(n);
            }

            // Remove classDef action blocks for now.
            using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                var nodes = engine.parseExpression(
                        @"//classDef/actionBlock",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                foreach (var n in nodes) TreeEdits.Delete(n);
            }

            // Let's take care of options first. That's because we can't
            // determine if this is a combined grammar or not.
            // Remove unused options at top of grammar def.
            // This specifically looks at the options at the top of the file,
            // not rule-based options. That will be handled separately below.
            using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                var options = engine.parseExpression(
                        @"//(fileOptionsSpec | parserOptionsSpec | lexerOptionsSpec | treeOptionsSpec)",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree).ToList();
                var nodes = engine.parseExpression(
                        @"//(fileOptionsSpec | parserOptionsSpec | lexerOptionsSpec | treeParserOptionsSpec)
                            /(option | lexerOption)
                                [id/*
                                        [
                                        text() = 'output'
                                        or text() = 'backtrack'
                                        or text() = 'buildAST'
                                        or text() = 'classHeaderSuffix'
                                        or text() = 'memoize'
                                        or text() = 'ASTLabelType'
                                        or text() = 'rewrite'
                                        or text() = 'k'
                                        or text() = 'exportVocab'
                                        or text() = 'testLiterals'
                                        or text() = 'interactive'
                                        or text() = 'charVocabulary'
                                        or text() = 'defaultErrorHandler'
                                        ]]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                TreeEdits.Delete(nodes);
                foreach (var opt in options)
                {
                    if (opt.ChildCount == 3)
                        TreeEdits.Delete(opt);
                }
            }

            // Delete rule options.
            using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                var nodes = engine.parseExpression(
                        @"//rule_/ruleOptionsSpec
                            /option
                                [id
                                    /(TOKEN_REF | RULE_REF)
                                        [text() = 'output'
                                        or text() = 'backtrack'
                                        or text() = 'memoize'
                                        or text() = 'ASTLabelType'
                                        or text() = 'rewrite'
                                        or text() = 'k'
                                        or text() = 'exportVocab'
                                        or text() = 'testLiterals'
                                        or text() = 'interactive'
                                        or text() = 'charVocabulary'
                                        or text() = 'defaultErrorHandler'
                                        ]]",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                TreeEdits.Delete(nodes);
                var options = engine.parseExpression(
                        @"//rule_/ruleOptionsSpec",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                foreach (var os in options)
                {
                    if (os.ChildCount == 3) TreeEdits.Delete(os);
                }
            }

            // Parser and Lexer in One Definition
            using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
            {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                var parser_nodes = engine.parseExpression(
                        @"//parserSpec",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                var lexer_nodes = engine.parseExpression(
                        @"//lexerSpec",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                var tree_nodes = engine.parseExpression(
                        @"//treeParserSpec",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                // Can this be a "combined" grammar? That can happen only if
                // one parser and one lexer decl and no tree decl. Options for
                // the lexer must be removed too.
                if (parser_nodes.Count() == 1 && lexer_nodes.Count() == 1 && tree_nodes.Count() == 0)
                {
                    var lexerSpec = lexer_nodes.First() as ANTLRv2Parser.LexerSpecContext;
                    if (lexerSpec.lexerOptionsSpec() == null)
                    {
                        // Nuke lexer class decl because it's a combined grammar.
                        TreeEdits.Delete(lexerSpec);
                        lexerSpec = null;
                    }
                    // Rewrite the parser spec.
                    var parserSpec = parser_nodes.First() as ANTLRv2Parser.ParserSpecContext;
                    var c = parserSpec.CLASS();
                    var i = parserSpec.id();
                    var e = parserSpec.EXTENDS();
                    var p = parserSpec.PARSER();
                    var s = parserSpec.superClass();
                    var new_sym = new TerminalNodeImpl(new CommonToken(ANTLRv4Lexer.GRAMMAR)
                    { Line = -1, Column = -1, Text = "grammar" });
                    text_before.TryGetValue(c as TerminalNodeImpl, out string v);
                    if (v != null)
                        text_before.Add(new_sym, v);
                    TreeEdits.Replace(c, (in IParseTree n, out bool cc) =>
                    {
                        cc = false;
                        return new_sym;
                    });
                    TreeEdits.Delete(e);
                    TreeEdits.Delete(p);
                    TreeEdits.Delete(s);
                }
            }

            if (StripLabelOperator)
            {
                using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
                {
                    org.eclipse.wst.xml.xpath2.processor.Engine engine =
                     new org.eclipse.wst.xml.xpath2.processor.Engine();
                    var nodes = engine.parseExpression(
                            @"//elementNoOptionSpec
                            /(id[following-sibling::COLON and not(following-sibling::id)]
                              | COLON[preceding-sibling::id and not(preceding-sibling::COLON)])",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { dynamicContext.Document })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree).ToList();
                    foreach (var n in nodes) TreeEdits.Delete(n);
                }
            }

            if (StripActionBlocks)
            {
                using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
                {
                    org.eclipse.wst.xml.xpath2.processor.Engine engine =
                     new org.eclipse.wst.xml.xpath2.processor.Engine();
                    var nodes = engine.parseExpression(
                            @"//actionBlock
                            [not(following-sibling::QM)]",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { dynamicContext.Document })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree).ToList();
                    foreach (var n in nodes) TreeEdits.Delete(n);
                }
            }

            // Remove syntactic predicates.
            {
                using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
                {
                    org.eclipse.wst.xml.xpath2.processor.Engine engine =
                     new org.eclipse.wst.xml.xpath2.processor.Engine();
                    var nodes = engine.parseExpression(
                            @"//ebnf
                            [SEMPREDOP]",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { dynamicContext.Document })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                    TreeEdits.Delete(nodes);
                }
            }

            // Convert double-quoted string literals to single quote.
            {
                using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
                {
                    org.eclipse.wst.xml.xpath2.processor.Engine engine =
                     new org.eclipse.wst.xml.xpath2.processor.Engine();
                    var nodes = engine.parseExpression(
                            @"//STRING_LITERAL",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { dynamicContext.Document })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                    foreach (var n in nodes)
                    {
                        var text = n.GetText();
                        if (text.Length == 0) continue;
                        if (text[0] != '"') continue;
                        text = text.Substring(1, text.Length - 2);
                        StringBuilder ss = new StringBuilder();
                        ss.Append("'");
                        foreach (var c in text)
                        {
                            if (c == '"') ss.Append("\\");
                            else ss.Append(c);
                        }
                        ss.Append("'");
                        var new_sym = new TerminalNodeImpl(new CommonToken(ANTLRv4Lexer.STRING_LITERAL)
                        { Line = -1, Column = -1, Text = ss.ToString() });
                        text_before.TryGetValue(n as TerminalNodeImpl, out string v);
                        if (v != null)
                            text_before.Add(new_sym, v);
                        TreeEdits.Replace(n, new_sym);
                    }
                }
            }

            // Convert "protected" to "fragment" for lexer symbols.
            //  Remove "protected" for parser symbols.
            {
                using (AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser))
                {
                    org.eclipse.wst.xml.xpath2.processor.Engine engine =
                     new org.eclipse.wst.xml.xpath2.processor.Engine();
                    var nodes = engine.parseExpression(
                            @"//rule_[id/RULE_REF]/(PROTECTED | PUBLIC | PRIVATE)",
                            new StaticContextBuilder()).evaluate(
                            dynamicContext, new object[] { dynamicContext.Document })
                        .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                    TreeEdits.Delete(nodes);
                }
            }

            // If the symbol occurs on the RHS of a parser rule, remove "protected".
            if (false) {
                org.eclipse.wst.xml.xpath2.processor.Engine engine =
                    new org.eclipse.wst.xml.xpath2.processor.Engine();
                AntlrTreeEditing.AntlrDOM.AntlrDynamicContext dynamicContext =
                    new AntlrTreeEditing.AntlrDOM.ConvertToDOM().Try(tree, parser);
                var nodes = engine.parseExpression(
                        @"//rule_
                            [not(id/TOKEN_REF/text() = //rule_[id/RULE_REF]/altList//TOKEN_REF/text())]
                            /(PROTECTED | PUBLIC | PRIVATE)",
                        new StaticContextBuilder()).evaluate(
                        dynamicContext, new object[] { dynamicContext.Document })
                    .Select(x => (x.NativeValue as AntlrTreeEditing.AntlrDOM.AntlrElement).AntlrIParseTree);
                TreeEdits.Delete(nodes);
            }

            StringBuilder sb = new StringBuilder();
            TreeEdits.Reconstruct(sb, tree, text_before);
            var new_code = sb.ToString();
            results.Add(new_ffn, new_code);
            results.Add(ffn.Replace(".g", ".txt"), errors.ToString());
        }
    }
}
