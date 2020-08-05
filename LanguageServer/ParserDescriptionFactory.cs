namespace LanguageServer
{
    using System.Collections.Generic;

    public class ParserDescriptionFactory
    {
        private static readonly IParserDescription _antlr4 = new Antlr4ParserDescription();
        private static readonly IParserDescription _antlr3 = new Antlr3ParserDescription();
        private static readonly IParserDescription _antlr2 = new Antlr2ParserDescription();
        public static List<string> AllLanguages
        {
            get
            {
                List<string> result = new List<string>
                {
                    _antlr2.Name,
                    _antlr3.Name,
                    _antlr4.Name
                };
                return result;
            }
        }

        public static IParserDescription Create(Workspaces.Document document)
        {
            if (document.ParseAs != null)
            {
                var parse_as = document.ParseAs;
                if (parse_as == "antlr2") return _antlr2;
                else if (parse_as == "antlr3") return _antlr3;
                else if (parse_as == "antlr4") return _antlr4;
                return null;
            }
            if (document.FullPath.EndsWith(".g2"))
            {
                document.ParseAs = "antlr2";
                return _antlr2;
            }
            else if (document.FullPath.EndsWith(".g3"))
            {
                document.ParseAs = "antlr3";
                return _antlr3;
            }
            else if (document.FullPath.EndsWith(".g4"))
            {
                document.ParseAs = "antlr4";
                return _antlr4;
            }
            else return null;
        }
    }
}
