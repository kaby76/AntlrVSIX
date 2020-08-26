namespace LanguageServer
{
    using System.Collections.Generic;

    public class Import
    {

        public static Dictionary<string, string> ImportGrammars(List<string> args)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            foreach (string f in args)
            {
                if (!System.IO.File.Exists(f)) continue;
                var input = System.IO.File.ReadAllText(f);
                if (f.EndsWith(".y"))
                {
                    var imp = new BisonImport();
                    imp.Try(f, input, ref results);
                }
                else if (f.EndsWith(".ebnf"))
                {
                    var imp = new W3CebnfImport();
                    imp.Try(f, input, ref results);
                }
                else if (f.EndsWith(".g2"))
                {
                    var imp = new Antlr2Import();
                    imp.Try(f, input, ref results);
                }
                else if (f.EndsWith(".g3"))
                {
                    var imp = new Antlr3Import();
                    imp.Try(f, input, ref results);
                }
            }
            return results;
        }
    }
}
