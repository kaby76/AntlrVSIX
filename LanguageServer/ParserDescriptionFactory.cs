namespace LanguageServer
{
    using System.Collections.Generic;

    public class ParserDescriptionFactory
    {
        private static Dictionary<Workspaces.Document, ParsingResults> _parsing_results = new Dictionary<Workspaces.Document, ParsingResults>();

        public static List<string> AllLanguages
        {
            get
            {
                List<string> result = new List<string>
                {
                    "antlr2",
                    "antlr3",
                    "antlr4",
                };
                return result;
            }
        }

        public static ParsingResults Create(Workspaces.Document document)
        {
            if (_parsing_results.ContainsKey(document) && _parsing_results[document] != null)
                return _parsing_results[document];

            ParsingResults result = null;

            if (document.ParseAs != null)
            {
                var parse_as = document.ParseAs;
                if (parse_as == "antlr2") result = new Antlr2ParsingResults(document);
                else if (parse_as == "antlr3") result = new Antlr3ParsingResults(document);
                else if (parse_as == "antlr4") result = new Antlr4ParsingResults(document);
                else if (parse_as == "bison") result = new BisonParsingResults(document);
                else if (parse_as == "ebnf") result = new W3CebnfParsingResults(document);
                else if (parse_as == "iso14977") result = new Iso14977ParsingResults(document);
                else if (parse_as == "lbnf") result = new lbnfParsingResults(document);
                else result = null;
            }
            else if (document.FullPath.EndsWith(".ebnf"))
            {
                document.ParseAs = "ebnf";
                result = new W3CebnfParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".g2"))
            {
                document.ParseAs = "antlr2";
                result = new Antlr2ParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".g3"))
            {
                document.ParseAs = "antlr3";
                result = new Antlr3ParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".g4"))
            {
                document.ParseAs = "antlr4";
                result = new Antlr4ParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".y"))
            {
                document.ParseAs = "bison";
                result = new BisonParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".iso14977"))
            {
                document.ParseAs = "iso14977";
                result = new Iso14977ParsingResults(document);
            }
            else if (document.FullPath.EndsWith(".cf"))
            {
                document.ParseAs = "lbnf";
                result = new Iso14977ParsingResults(document);
            }
            else result = null;
            _parsing_results[document] = result;
            return result;
        }
    }
}
