grammar R;

prog
    : expr_or_assign* ;

expr_or_assign
@after {System.out.println(getRuleInvocationStack());}
    : expr '++'
    | expr	// match ID a, fall out, reenter, match "(i)<-x" via alt 1
                // it thinks it's same context from prog, but it's not; it's
                // 2nd time through expr_or_assign* loop.
    ;

expr
    : expr_primary ('<-' ID)?
    ;

expr_primary
    : '(' ID ')'
    | ID '(' ID ')'
    | ID
    ;

HEX
    : '0' ('x'|'X') HEXDIGIT+ [Ll]?
    ;

INT
    : DIGIT+ [Ll]?
    ;

fragment
HEXDIGIT
    : ('0'..'9'|'a'..'f'|'A'..'F')
    ;

FLOAT
    : DIGIT+ '.' DIGIT* EXP? [Ll]?
    | DIGIT+ EXP? [Ll]?
    | '.' DIGIT+ EXP? [Ll]?
    ;

fragment
DIGIT
    : '0'..'9'
    ;

fragment
EXP
    : ('E' | 'e') ('+' | '-')? INT
    ;

COMPLEX
    : INT 'i'
    | FLOAT 'i'
    ;

STRING
    : '"' ( ESC | ~('\\'|'"') )* '"'
    | '\'' ( ESC | ~('\\'|'\'') )* '\''
    ;

fragment
ESC
    : '\\' ([abtnfrv]|'"'|'\'')
    | UNICODE_ESCAPE
    | HEX_ESCAPE
    | OCTAL_ESCAPE
    ;

fragment
UNICODE_ESCAPE
    : '\\' 'u' HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT
    | '\\' 'u' '{' HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT '}'
    ;

fragment
OCTAL_ESCAPE
    : '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    | '\\' ('0'..'7') ('0'..'7')
    | '\\' ('0'..'7')
    ;

fragment
HEX_ESCAPE
    : '\\' HEXDIGIT HEXDIGIT?
    ;

ID
    : '.'? (LETTER|'_'|'.') (LETTER|DIGIT|'_'|'.')*
    |   LETTER (LETTER|DIGIT|'_'|'.')*
    ;

fragment
LETTER
    : 'a'..'z'|'A'..'Z'|'\u0080'..'\u00FF'
    ;

USER_OP
    : '%' .* '%'
    ;

COMMENT
    : '#' .* '\n' {skip();}
    ;

WS
    : (' '|'\t'|'\n'|'\r')+ {skip();}
    ;
