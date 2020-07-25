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
                    BisonImport.Try(f, input, ref results);
                }
                else if (f.EndsWith(".g3") || f.EndsWith(".g"))
                {
                    LanguageServer.Antlr3Import.Try(f, input, ref results);
                }
            }
            return results;
        }
    }
}
