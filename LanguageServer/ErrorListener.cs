using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using System.Linq;

    public class ErrorListener<S> : ConsoleErrorListener<S>
    {
        public bool had_error;
        private Parser _parser;
        private Lexer _lexer;
        private CommonTokenStream _token_stream;
        private bool _first_time;

        public ErrorListener(Parser parser, Lexer lexer, CommonTokenStream token_stream)
        {
            _parser = parser;
            _lexer = lexer;
            _token_stream = token_stream;
            _first_time = true;
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, S offendingSymbol, int line,
            int col, string msg, RecognitionException e)
        {
            had_error = true;
            //base.SyntaxError(output, recognizer, offendingSymbol, line, col, msg, e);
        }
    }
}
