// ================================

lexer grammar AntlrLexer;

tokens {
    TOKEN_REF,
    RULE_REF,
    LEXER_CHAR_SET
}

@members {
    private int _currentRuleType = Token.INVALID_TYPE;

    public int getCurrentRuleType() {
        return _currentRuleType;
    }

    public void setCurrentRuleType(int ruleType) {
        this._currentRuleType = ruleType;
    }

    protected void handleBeginArgument() {
        if (inLexerRule()) {
            pushMode(LexerCharSet);
            more();
        }
        else {
            pushMode(ArgAction);
            more();
        }
    }

    @Override
    public Token emit() {
        if (_type == TOKEN_REF || _type==RULE_REF ) {
            if (_currentRuleType == Token.INVALID_TYPE) { // if outside of rule def
                _currentRuleType = _type;                 // set to inside lexer or parser rule
            }
        }
        else if (_type == SEMI) {                  // exit rule def
            _currentRuleType = Token.INVALID_TYPE;
        }

        return super.emit();
    }

    private boolean inLexerRule() {
        return _currentRuleType == TOKEN_REF;
    }
    private boolean inParserRule() { // not used, but added for clarity
        return _currentRuleType == RULE_REF;
    }
}

DOC_COMMENT
    :   '/**' .*? ('*/' | EOF)
    ;

BLOCK_COMMENT
    :   '/*' .*? ('*/' | EOF)  -> channel(HIDDEN)
    ;

LINE_COMMENT
    :   '//' ~[\r\n]*  -> channel(HIDDEN)
    ;

BEGIN_ARG_ACTION
    :   '[' {handleBeginArgument();}
    ;

// OPTIONS and TOKENS must also consume the opening brace that captures
// their option block, as this is the easiest way to parse it separate
// to an ACTION block, despite it using the same {} delimiters.
//
OPTIONS      : 'options'  [ \t\f\n\r]* '{'  ;
TOKENS       : 'tokens'   [ \t\f\n\r]* '{'  ;
CHANNELSx    : 'channels' [ \t\f\n\r]* '{'  ;

IMPORT       : 'import'               ;
FRAGMENT     : 'fragment'             ;
LEXER        : 'lexer'                ;
PARSER       : 'parser'               ;
GRAMMAR      : 'grammar'              ;
PROTECTED    : 'protected'            ;
PUBLIC       : 'public'               ;
PRIVATE      : 'private'              ;
RETURNS      : 'returns'              ;
LOCALS       : 'locals'               ;
THROWS       : 'throws'               ;
CATCH        : 'catch'                ;
FINALLY      : 'finally'              ;
MODE         : 'mode'                 ;

COLON        : ':'                    ;
COLONCOLON   : '::'                   ;
COMMA        : ','                    ;
SEMI         : ';'                    ;
LPAREN       : '('                    ;
RPAREN       : ')'                    ;
RARROW       : '->'                   ;
LT           : '<'                    ;
GT           : '>'                    ;
ASSIGN       : '='                    ;
QUESTION     : '?'                    ;
STAR         : '*'                    ;
PLUS         : '+'                    ;
PLUS_ASSIGN  : '+='                   ;
OR           : '|'                    ;
DOLLAR       : '$'                    ;
DOT          : '.'                    ;
RANGE        : '..'                   ;
AT           : '@'                    ;
POUND        : '#'                    ;
NOT          : '~'                    ;
RBRACE       : '}'                    ;

/** Allow unicode rule/token names */
//ID    :   NameStartChar NameChar*;
// ##################### to allow testing ANTLR grammars in intellij preview
RULE_REF  : [a-z][a-zA-Z_0-9]* ;
TOKEN_REF : [A-Z][a-zA-Z_0-9]* ;


fragment
NameChar
    :   NameStartChar
    |   '0'..'9'
    |   '_'
    |   '\u00B7'
    |   '\u0300'..'\u036F'
    |   '\u203F'..'\u2040'
    ;

fragment
NameStartChar
    :   'A'..'Z'
    |   'a'..'z'
    |   '\u00C0'..'\u00D6'
    |   '\u00D8'..'\u00F6'
    |   '\u00F8'..'\u02FF'
    |   '\u0370'..'\u037D'
    |   '\u037F'..'\u1FFF'
    |   '\u200C'..'\u200D'
    |   '\u2070'..'\u218F'
    |   '\u2C00'..'\u2FEF'
    |   '\u3001'..'\uD7FF'
    |   '\uF900'..'\uFDCF'
    |   '\uFDF0'..'\uFFFD'
    ; // ignores | ['\u10000-'\uEFFFF] ;

INT : [0-9]+
    ;

// ANTLR makes no distinction between a single character literal and a
// multi-character string. All literals are single quote delimited and
// may contain unicode escape sequences of the form \uxxxx, where x
// is a valid hexadecimal number (as per Java basically).
STRING_LITERAL
    :  '\'' (ESC_SEQ | ~['\r\n\\])* '\''
    ;

UNTERMINATED_STRING_LITERAL
    :  '\'' (ESC_SEQ | ~['\r\n\\])*
    ;

// Any kind of escaped character that we can embed within ANTLR
// literal strings.
fragment
ESC_SEQ
    :   '\\'
        (   // The standard escaped character set such as tab, newline, etc.
            [btnfr"'\\]
        |   // A Java style Unicode escape sequence
            UNICODE_ESC
        |   // Invalid escape
            .
        |   // Invalid escape at end of file
            EOF
        )
    ;

fragment
UNICODE_ESC
    :   'u' (HEX_DIGIT (HEX_DIGIT (HEX_DIGIT HEX_DIGIT?)?)?)?
    ;

fragment
HEX_DIGIT : [0-9a-fA-F] ;

WS  :   [ \t\r\n\f]+ -> channel(HIDDEN) ;

// Many language targets use {} as block delimiters and so we
// must recursively match {} delimited blocks to balance the
// braces. Additionally, we must make some assumptions about
// literal string representation in the target language. We assume
// that they are delimited by ' or " and so consume these
// in their own alts so as not to inadvertantly match {}.

ACTION
    :   '{'
        (   ACTION
        |   ACTION_ESCAPE
        |   ACTION_STRING_LITERAL
        |   ACTION_CHAR_LITERAL
        |   '/*' .*? '*/' // ('*/' | EOF)
        |   '//' ~[\r\n]*
        |   .
        )*?
        ('}'|EOF)
    ;

fragment
ACTION_ESCAPE
        :   '\\' .
        ;

fragment
ACTION_STRING_LITERAL
        :   '"' (ACTION_ESCAPE | ~["\\])* '"'
        ;

fragment
ACTION_CHAR_LITERAL
        :   '\'' (ACTION_ESCAPE | ~['\\])* '\''
        ;

// -----------------
// Illegal Character
//
// This is an illegal character trap which is always the last rule in the
// lexer specification. It matches a single character of any value and being
// the last rule in the file will match when no other rule knows what to do
// about the character. It is reported as an error but is not passed on to the
// parser. This means that the parser to deal with the gramamr file anyway
// but we will not try to analyse or code generate from a file with lexical
// errors.
//
ERRCHAR
    :   .   -> channel(HIDDEN)
    ;

mode ArgAction; // E.g., [int x, List<String> a[]]

    NESTED_ARG_ACTION
        :   '['                         -> more, pushMode(ArgAction)
        ;

    ARG_ACTION_ESCAPE
        :   '\\' .                      -> more
        ;

    ARG_ACTION_STRING_LITERAL
        :   ('"' ('\\' . | ~["\\])* '"')-> more
        ;

    ARG_ACTION_CHAR_LITERAL
        :   ('"' '\\' . | ~["\\] '"')   -> more
        ;

    ARG_ACTION
        :   ']'                         -> popMode
        ;

    UNTERMINATED_ARG_ACTION // added this to return non-EOF token type here. EOF did something weird
        :   EOF                         -> popMode
        ;

    ARG_ACTION_CHAR // must be last
        :   .                           -> more
        ;


mode LexerCharSet;

    LEXER_CHAR_SET_BODY
        :   (   ~[\]\\]
            |   '\\' .
            )
                                        -> more
        ;

    LEXER_CHAR_SET
        :   ']'                         -> popMode
        ;

    UNTERMINATED_CHAR_SET
        :   EOF                         -> popMode
        ;
