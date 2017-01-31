using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace AntlrParserTool
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonTokenStream cts = new CommonTokenStream(
                new ANTLRv4Lexer(
                    new AntlrInputStream(
                        new StreamReader(
                            args[0]).ReadToEnd())));
            var ant_parser = new ANTLRv4Parser(cts);
            ant_parser.grammarSpec();
        }
    }
}
