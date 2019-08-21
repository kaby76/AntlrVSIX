using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.Grammar
{
    class GrammarDescriptionFactory
    {
        public static IGrammarDescription Create(string ffn)
        {
            if (Java.JavaGrammarDescription.Instance.IsFileType(ffn))
                return Java.JavaGrammarDescription.Instance;
            else if (Antlr.AntlrGrammarDescription.Instance.IsFileType(ffn))
                return Antlr.AntlrGrammarDescription.Instance;
            else return null;
        }

    }
}
