namespace LanguageServer
{
    using System.Collections.Generic;

    public class ParsingResultsFactory
    {
        private static readonly Dictionary<string, ParsingResults> _per_file_parser_details = new Dictionary<string, ParsingResults>();

        public static IEnumerable<KeyValuePair<string, ParsingResults>> AllParserDetails => _per_file_parser_details;

        public static ParsingResults Create(Workspaces.Document document)
        {
            if (document == null)
            {
                return null;
            }

            string ffn = document.FullPath;
            foreach (KeyValuePair<string, ParsingResults> pd in _per_file_parser_details)
            {
                if (pd.Key == ffn)
                {
                    return pd.Value;
                }
            }
            IParserDescription gd = ParserDescriptionFactory.Create(document);
            if (gd == null)
            {
                return null;
            }

            ParsingResults result = gd.CreateParserDetails(document);
            result.Gd = gd;
            _per_file_parser_details[ffn] = result;
            return result;
        }
    }
}
