namespace LanguageServer
{
    //using Options;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class GrammarDescriptionFactory
    {
        private static readonly IGrammarDescription _antlr = new AntlrGrammarDescription();
        public static List<string> AllLanguages
        {
            get
            {
                List<string> result = new List<string>
                {
                    _antlr.Name
                };
                return result;
            }
        }

        public static IGrammarDescription Create(string ffn)
        {
            if (_antlr.IsFileType(ffn))
            {
                return _antlr;
            }
            return null;
        }
    }
}
