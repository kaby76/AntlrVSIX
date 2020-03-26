lexer grammar FlexLexer;

channels {
	OFF_CHANNEL		// non-default channel for whitespace and comments
}

tokens {
SECTEND,
SCDECL,
XSCDECL,
NAME,
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

fragment OptWs :
	[ \t]*
	;

fragment Ws :
	[ \t]+
	;

fragment Nl :
	[\r]?[\n]
	;

fragment Name :
	NameStartChar NameChar*
	;

fragment NameChar
	:	NameStartChar
	|	'0'..'9'
	|	'_'
	|	'\u00B7'
	|	'\u0300'..'\u036F'
	|	'\u203F'..'\u2040'
	| '.'
	| '-'
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


mode SECT2;

	BlankLine : OptWs Nl // { input.LA(1); }
		-> channel(OFF_CHANNEL);

