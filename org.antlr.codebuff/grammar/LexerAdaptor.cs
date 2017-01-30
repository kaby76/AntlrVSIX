using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace AntlrVSIX.Grammar
{
    public abstract class LexerAdaptor : Lexer
    {
        private ICharStream _input;

        public LexerAdaptor(ICharStream input)
            : base(input)
        {
            _input = input;
        }

        /**
         * Track whether we are inside of a rule and whether it is lexical parser. _currentRuleType==Token.INVALID_TYPE
         * means that we are outside of a rule. At the first sign of a rule name reference and _currentRuleType==invalid, we
         * can assume that we are starting a parser rule. Similarly, seeing a token reference when not already in rule means
         * starting a token rule. The terminating ';' of a rule, flips this back to invalid type.
         *
         * This is not perfect logic but works. For example, "grammar T;" means that we start and stop a lexical rule for
         * the "T;". Dangerous but works.
         *
         * The whole point of this state information is to distinguish between [..arg actions..] and [charsets]. Char sets
         * can only occur in lexical rules and arg actions cannot occur.
         */
        private int _currentRuleType = TokenConstants.InvalidType;

        public int getCurrentRuleType()
        {
            return _currentRuleType;
        }

        public void setCurrentRuleType(int ruleType)
        {
            this._currentRuleType = ruleType;
        }

        protected void handleBeginArgument()
        {
            if (inLexerRule())
            {
                this.PushMode(ANTLRv4Lexer.LexerCharSet);
                this.More();
            }
            else
            {
                this.PushMode(ANTLRv4Lexer.ArgAction);
            }
        }

        public override IToken Emit()
        {
            if (this.Type == ANTLRv4Lexer.TOKEN_REF || this.Type == ANTLRv4Lexer.RULE_REF)
            {
                if (this.getCurrentRuleType() == TokenConstants.InvalidType)
                    _currentRuleType = this.Type;
            }
            else if (this.Type == ANTLRv4Lexer.SEMI)
            {
                _currentRuleType = TokenConstants.InvalidType;
            }

            return base.Emit();
        }

        private bool inLexerRule()
        {
            return _currentRuleType == ANTLRv4Lexer.TOKEN_REF;
        }


        private bool inParserRule()
        { // not used, but added for clarity
            return _currentRuleType == ANTLRv4Lexer.RULE_REF;
        }
    }
}

