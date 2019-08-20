namespace AntlrVSIX.Grammar
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using System;
    using System.Linq;

    public abstract class LexerAdaptor : Lexer
    {
        private ICharStream _input;

        public LexerAdaptor(
            Antlr4.Runtime.ICharStream input,
            System.IO.TextWriter output,
            System.IO.TextWriter errorOutput)
            : base(input, output, errorOutput)
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
                this.PushMode(ANTLRv4Lexer.MLexerCharSet);
                this.More();
            }
            else
            {
                this.PushMode(ANTLRv4Lexer.MArgument);
            }
        }

        protected void handleEndArgument()
        {
            this.PopMode();
            if (ModeStack.Any())
            {
                this.Type = ANTLRv4Lexer.ARGUMENT_CONTENT;
            }
        }

        protected void handleEndAction()
        {
            this.PopMode();
            if (ModeStack.Any())
            {
                this.Type = ANTLRv4Lexer.ACTION_CONTENT;
            }
        }

        public override IToken Emit()
        {
            if (this.Type == ANTLRv4Lexer.ID)
            {
                String firstChar = this._input.GetText(
                    Interval.Of(this.TokenStartCharIndex, this.TokenStartCharIndex));

                if (Char.IsUpper(firstChar.ElementAt(0)))
                {
                    this.Type = ANTLRv4Lexer.TOKEN_REF;
                }
                else
                {
                    this.Type = ANTLRv4Lexer.RULE_REF;
                }

                if (_currentRuleType == TokenConstants.InvalidType)
                { // if outside of rule def
                    _currentRuleType = this.Type; // set to inside lexer or parser rule
                }
            }
            else if (this.Type == ANTLRv4Lexer.SEMI)
            { // exit rule def
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

