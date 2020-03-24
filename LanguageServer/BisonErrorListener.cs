// Template generated code from Antlr4BuildTasks.Template v 3.0
namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class BisonErrorListener<S> : ConsoleErrorListener<S>
    {
        public bool had_error;
        private readonly Parser _parser;
        private readonly Lexer _lexer;
        private readonly CommonTokenStream _token_stream;
        private bool _first_time;
        private StringBuilder _errors;

        public BisonErrorListener(Parser parser, Lexer lexer, CommonTokenStream token_stream, StringBuilder errors)
        {
            _parser = parser;
            _lexer = lexer;
            _token_stream = token_stream;
            _first_time = true;
            _errors = errors;
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, S offendingSymbol, int line,
            int col, string msg, RecognitionException e)
        {
            if (! had_error)
            {
                _errors.AppendLine("Parse of Bison file "
                    + _token_stream.SourceName
                    + " failed. Please correct and try again.");
            }
            had_error = true;
        }
    }
}