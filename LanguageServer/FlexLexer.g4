lexer grammar FlexLexer;

channels {
	OFF_CHANNEL		// non-default channel for whitespace and comments
}

options {
	superClass = FlexLexerAdaptor ;
}

tokens {
SECTEND,
SCDECL,
XSCDECL,
TOK_OPTION,
TOK_OUTFILE,
TOK_EXTRA_TYPE,
TOK_PREFIX,
TOK_YYCLASS,
TOK_HEADER_FILE,
TOK_TABLES_FILE,
EOF_OP,
BEGIN_REPEAT_POSIX,
NUMBER,
END_REPEAT_POSIX,
BEGIN_REPEAT_FLEX,
END_REPEAT_FLEX,
PREVCCL,
CHAR,
CCE_ALNUM,
CCE_ALPHA,
CCE_BLANK,
CCE_CNTRL,
CCE_DIGIT,
CCE_GRAPH,
CCE_LOWER,
CCE_PRINT,
CCE_PUNCT,
CCE_SPACE,
CCE_XDIGIT,
CCE_UPPER,
CCE_NEG_ALNUM,
CCE_NEG_ALPHA,
CCE_NEG_BLANK,
CCE_NEG_CNTRL,
CCE_NEG_DIGIT,
CCE_NEG_GRAPH,
CCE_NEG_PRINT,
CCE_NEG_PUNCT,
CCE_NEG_SPACE,
CCE_NEG_XDIGIT,
CCE_NEG_LOWER,
CCE_NEG_UPPER,
ERROR,
CCL_OP_DIFF,
CCL_OP_UNION
}

// ======================= Common fragments =========================

fragment Underscore
	: '_'
	;

fragment NameStartChar
	:	'A'..'Z'
	|	'a'..'z'
    | '_'
	|	'\u00C0'..'\u00D6'
	|	'\u00D8'..'\u00F6'
	|	'\u00F8'..'\u02FF'
	|	'\u0370'..'\u037D'
	|	'\u037F'..'\u1FFF'
	|	'\u200C'..'\u200D'
	|	'\u2070'..'\u218F'
	|	'\u2C00'..'\u2FEF'
	|	'\u3001'..'\uD7FF'
	|	'\uF900'..'\uFDCF'
	|	'\uFDF0'..'\uFFFD'
	| '$' // For PHP
	;	// ignores | ['\u10000-'\uEFFFF] ;

fragment DQuoteLiteral
	: DQuote ( EscSeq | ~["\r\n\\] | ( '\\' [\n\r]*) )* DQuote
	;

fragment DQuote
	: '"'
	;

fragment SQuote
	: '\''
	;

fragment CharLiteral
	: SQuote ( EscSeq | ~['\r\n\\] )  SQuote
	;

fragment SQuoteLiteral
	: SQuote ( EscSeq | ~['\r\n\\] )* SQuote
	;

fragment Esc
	: '\\'
	;

fragment EscSeq
	:	Esc
		([abefnrtv?"'\\]	// The standard escaped character set such as tab, newline, etc.
		| [xuU]?[0-9]+) // C-style 
	;

fragment EscAny
	:	Esc .
	;

fragment Id
	: NameStartChar NameChar*
	;

fragment Type
	: ([\t\r\n\f a-zA-Z0-9] | '[' | ']' | '{' | '}' | '.' | '_' | '(' | ')' | ',')+
	;

fragment NameChar
	:	NameStartChar
	|	'0'..'9'
	|	Underscore
	|	'\u00B7'
	|	'\u0300'..'\u036F'
	|	'\u203F'..'\u2040'
	| '.'
	| '-'
	;

fragment BlockComment
	: '/*' 
     (
	   ('/' ~'*')
	   | ~'/'
	 )*
	 '*/'
	;

fragment LineComment
	: '//' ~[\r\n]*
	;

fragment LineCommentExt
	: '//' ~'\n'* ( '\n' Hws* '//' ~'\n'* )*
	;

fragment Ws
	: Hws
	| Vws
	;

fragment Hws
	: [ \t]
	;

fragment Vws
	: [\r\n\f]
	;

/* Four types of user code:
    - prologue (code between '%{' '%}' in the first section, before %%);
    - actions, printers, union, etc, (between braced in the middle section);
    - epilogue (everything after the second %%).
    - predicate (code between '%?{' and '{' in middle section); */

// -------------------------
// Actions

fragment LBrace
	: '{'
	;

fragment RBrace
	: '}'
	;

fragment PercentLBrace
	: '%{'
	;

fragment PercentRBrace
	: '%}'
	;

fragment PercentQuestion
	: '%?{'
	;

fragment ActionCode
    : Stuff*
	;

fragment Stuff
	: EscAny
	| DQuoteLiteral
	| SQuoteLiteral
	| BlockComment
	| LineComment
	| NestedAction
	| ~('{' | '}' | '\'' | '"')
	;

fragment NestedPrologue
	: PercentLBrace ActionCode PercentRBrace
	;

fragment NestedAction
	: LBrace ActionCode RBrace
	;

fragment NestedPredicate
	: PercentQuestion ActionCode RBrace
	;

fragment Sp
	: Ws*
	;

fragment Eqopt
	: (Sp [=])?
	;

fragment Nl :
    [\r]?[\n]
	;

fragment OptWs :
    [ \t]*
    ;

PercentPercent:   '%%'
		{
			++percent_percent_count;
			if (percent_percent_count == 1)
			{
				//this.PushMode(BisonLexer.RuleMode);
				return;
			} else if (percent_percent_count == 2)
			{
				this.PushMode(BisonLexer.EpilogueMode);
				return;
			} else
			{
				this.Type = BisonLexer.PERCENT_PERCENT;
				return;
			}
		}
		;


// -------------------------
// Comments

BLOCK_COMMENT
	:	BlockComment	-> channel(OFF_CHANNEL)
	;

LINE_COMMENT
	:	LineComment		-> channel(OFF_CHANNEL)
	;


OPEN_BRACKET : '[' ;
CLOSE_BRACKET : ']' ;
DQUOTE : '"' ;
EQUAL : '=' ;
OPEN_CURLY : '{' ;
CLOSE_CURLY : '}' ;
NL : Nl ;
UP : '^' ;
LT : '<' ;
GT : '>' ;
STAR : '*' ;
COMMA : ',' ;
DOLLAR : '$' ;
VBAR : '|' ;
SLASH : '/' ;
PLUS : '+' ;
QUESTION : '?' ;
DOT : '.' ;
OPEN_PAREN : '(' ;
CLOSE_PAREN : ')' ;
MINUS : '-' ;

mode INITIAL;

SCDECL : '%s' Id? ;
XSCDECL : '%x' Id? ;
NAME : Id ;

mode SECT2;

	BlankLine : OptWs Nl // { input.LA(1); }
		-> channel(OFF_CHANNEL);

