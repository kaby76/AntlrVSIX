namespace LanguageServer
{
    using System.Collections.Generic;

    public class Import
    {

        private static void Try(string input, ref Dictionary<string, string> results)
        {
            if (input.EndsWith(".y"))
            {
                BisonImport.Try(input, ref results);
            }
            else if (input.EndsWith(".g3") || input.EndsWith(".g"))
            {
                LanguageServer.Antlr3Import.Try(input, ref results);
            }
        }

        public static Dictionary<string, string> ImportGrammars(List<string> args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string f in args)
            {
                Try(f, ref result);
            }
            return result;
        }
    }
}
