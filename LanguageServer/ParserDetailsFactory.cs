namespace LanguageServer
{
    using System.Collections.Generic;

    public class ParserDetailsFactory
    {
        static Dictionary<string, ParserDetails> _per_file_parser_details = new Dictionary<string, ParserDetails>();

        public static IEnumerable<KeyValuePair<string, ParserDetails>> AllParserDetails
        {
            get { return _per_file_parser_details; }
        }

        public static ParserDetails Create(Workspaces.Document item)
        {
            if (item == null) return null;
            string ffn = item.FullPath;
            foreach (var pd in _per_file_parser_details)
            {
                if (pd.Key == ffn) return pd.Value;
            }
            IGrammarDescription gd = GrammarDescriptionFactory.Create(ffn);
            if (gd == null) return null;
            var result = gd.CreateParserDetails(item);
            result.Gd = gd;
            _per_file_parser_details[ffn] = result;
            return result;
        }
    }
}
