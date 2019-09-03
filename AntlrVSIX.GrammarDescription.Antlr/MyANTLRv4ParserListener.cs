using System;
using System.Collections.Generic;
using System.Text;

namespace AntlrVSIX.GrammarDescription.Antlr
{
    class MyANTLRv4ParserListener : ANTLRv4ParserBaseListener
    {
 
        public override void ExitLexerRuleSpec(ANTLRv4Parser.LexerRuleSpecContext context)
        {
            base.ExitLexerRuleSpec(context);
        }

        public override void ExitParserRuleSpec(ANTLRv4Parser.ParserRuleSpecContext context)
        {
            base.ExitParserRuleSpec(context);
        }

        public override void ExitGrammarSpec(ANTLRv4Parser.GrammarSpecContext context)
        {
            base.ExitGrammarSpec(context);
        }
    }
}
