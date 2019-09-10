lexer grammar RustLexer;

channels {
	OFF_CHANNEL		// non-default channel for whitespace and comments
}

import xidstart, xidcontinue;

UNDERSCORE: '_';
ABSTRACT: 'abstract';
ALIGNOF: 'alignof';
AS: 'as';
AUTO: 'auto';
BECOME: 'become';
BOX: 'box';
BREAK: 'break';
CATCH: 'catch';
CONST: 'const';
CONTINUE: 'continue';
CRATE: 'crate';
DEFAULT: 'default';
DO: 'do';
ELSE: 'else';
ENUM: 'enum';
EXTERN: 'extern';
FALSE: 'false';
FINAL: 'final';
FN: 'fn';
FOR: 'for';
IF: 'if';
IMPL: 'impl';
IN: 'in';
LET: 'let';
LOOP: 'loop';
MACRO: 'macro';
MATCH: 'match';
MOD: 'mod';
MOVE: 'move';
MUT: 'mut';
OFFSETOF: 'offsetof';
OVERRIDE: 'override';
PRIV: 'priv';
PROC: 'proc';
PURE: 'pure';
PUB: 'pub';
REF: 'ref';
RETURN: 'return';
SELF: 'self';
CSELF: 'Self';
SIZEOF: 'sizeof';
STATIC: 'static';
STRUCT: 'struct';
SUPER: 'super';
TRAIT: 'trait';
TRUE: 'true';
TYPE: 'type';
TYPEOF: 'typeof';
UNION: 'union';
UNSAFE: 'unsafe';
UNSIZED: 'unsized';
USE: 'use';
VIRTUAL: 'virtual';
WHERE: 'where';
WHILE: 'while';
YIELD: 'yield';

SEMI: ';';
CO: ',';
DOTDOTDOT: '...';
DOTDOT: '..';
DOT: '.';
OP: '(';
CP: ')';
OC: '{';
CC: '}';
OB: '[';
CB: ']';
AT: '@';

SQ: '~';
COCO: '::';
COL: ':';
DOL: '$';
QU: '?';

EQEQ: '==';
FAT_ARROW: '=>';
EQ: '=';
NE: '!=';
NOT: '!';
LE: '<=';
SHL: '<<';
SHLEQ: '<<=';
LT: '<';
GE: '>=';
SHR: '>>';
SHREQ: '>>=';
GT: '>';


LARROW: '<-';
RARROW: '->';
MINUS: '-';
MINUSEQ: '-=';
ANDAND: '&&';
AND: '&';
ANDEQ: '&=';
OROR: '||';
OR: '|';
OREQ: '|=';
PLUS: '+';
PLUSEQ: '+=';
STAR: '*';
STAREQ: '*=';
SLASH: '/';
SLASHEQ: '/=';
CARET: '^';
CARETEQ: '^=';
PERCENT: '%';
PERCENTEQ: '%=';

POUND: '#';

SHEBANG_LINE:  { this.Line == 1 && this.Column == 0 }? '#!' (~[\r\n])+ [\n\r] ;

// `$` is recognized as a token, so it may be present in token trees,
// and `macro_rules!` makes use of it. But it is not mentioned anywhere
// else in this grammar.
CashMoney:
    DOL;

fragment IDENT:
    XID_Start XID_Continue*;

Lifetime:
    [']IDENT;

Ident:
    IDENT;

fragment SIMPLE_ESCAPE:
    '\\' [0nrt'"\\];

fragment CHAR:
    ~['"\r\n\\\ud800-\udfff]          // a single BMP character other than a backslash, newline, or quote
    | [\ud800-\udbff][\udc00-\udfff]  // a single non-BMP character (hack for Java)
    | SIMPLE_ESCAPE
    | '\\x' [0-7] [0-9a-fA-F]
    | '\\u{' [0-9a-fA-F]+ '}';

CharLit:
    '\'' (CHAR | '"') '\'';

fragment OTHER_STRING_ELEMENT:
    '\''
    | '\\' '\r'? '\n' [ \t]*
    | '\r'
    | '\n';

fragment STRING_ELEMENT:
    CHAR
    | OTHER_STRING_ELEMENT;

fragment RAW_CHAR:
    ~[\ud800-\udfff]          // any BMP character
    | [\ud800-\udbff][\udc00-\udfff];  // any non-BMP character (hack for Java)

// Here we use a non-greedy match to implement the
// (non-regular) rules about raw string syntax.
fragment RAW_STRING_BODY:
    '"' RAW_CHAR*? '"'
    | '#' RAW_STRING_BODY '#';

StringLit:
    '"' STRING_ELEMENT* '"'
    | 'r' RAW_STRING_BODY;

fragment BYTE:
    ' '               // any ASCII character from 32 (space) to 126 (`~`),
    | '!'             // except 34 (double quote), 39 (single quote), and 92 (backslash)
    | [#-&]
    | [(-[]
    | ']'
    | '^'
    | [_-~]
    | SIMPLE_ESCAPE
    | '\\x' [0-9a-fA-F][0-9a-fA-F];

ByteLit:
    'b\'' (BYTE | '"') '\'';

fragment BYTE_STRING_ELEMENT:
    BYTE
    | OTHER_STRING_ELEMENT;

fragment RAW_BYTE_STRING_BODY:
    '"' [\t\r\n -~]*? '"'
    | '#' RAW_BYTE_STRING_BODY '#';

ByteStringLit:
    'b"' BYTE_STRING_ELEMENT* '"'
    | 'br' RAW_BYTE_STRING_BODY;

fragment DEC_DIGITS:
    [0-9][0-9_]*;

// BareIntLit and FullIntLit both match '123'; BareIntLit wins by virtue of
// appearing first in the file. (This comment is to point out the dependency on
// a less-than-obvious ANTLR rule.)
BareIntLit:
    DEC_DIGITS;

fragment INT_SUFFIX:
    [ui] ('8'|'16'|'32'|'64'|'size');

FullIntLit:
    DEC_DIGITS INT_SUFFIX?
    | '0x' '_'* [0-9a-fA-F] [0-9a-fA-F_]* INT_SUFFIX?
    | '0o' '_'* [0-7] [0-7_]* INT_SUFFIX?
    | '0b' '_'* [01] [01_]* INT_SUFFIX?;

fragment EXPONENT:
    [Ee] [+-]? '_'* [0-9] [0-9_]*;

fragment FLOAT_SUFFIX:
    'f32'
    | 'f64';

// Some lookahead is required here. ANTLR does not support this
// except by injecting some Java code into the middle of the pattern.
//
// A floating-point literal may end with a dot, but:
//
// *   `100..f()` is parsed as `100 .. f()`, not `100. .f()`,
//     contrary to the usual rule that lexers are greedy.
//
// *   Similarly, but less important, a letter or underscore after `.`
//     causes the dot to be interpreted as a separate token by itself,
//     so that `1.abs()` parses a method call. The type checker will
//     later reject it, though.
//
FloatLit:
    DEC_DIGITS '.' [0-9] [0-9_]* EXPONENT? FLOAT_SUFFIX?
    | DEC_DIGITS ('.' {
        /* dot followed by another dot is a range, not a float */
        this.InputStream.LA(1) != '.' &&
        /* dot followed by an identifier is an integer with a function call, not a float */
        this.InputStream.LA(1) != '_' &&
        !(this.InputStream.LA(1) >= 'a' && this.InputStream.LA(1) <= 'z') &&
        !(this.InputStream.LA(1) >= 'A' && this.InputStream.LA(1) <= 'Z')
    }?)
    | DEC_DIGITS EXPONENT FLOAT_SUFFIX?
    | DEC_DIGITS FLOAT_SUFFIX;

Whitespace:
    [ \t\r\n]+ -> channel(OFF_CHANNEL);
	
LineComment:
    '//' ~[\r\n]* -> channel(OFF_CHANNEL);

BlockComment:
    '/*' (~[*/] | '/'* BlockComment | '/'+ (~[*/]) | '*'+ ~[*/])* '*'+ '/' -> channel(OFF_CHANNEL);

// BUG: only ascii identifiers are permitted
// BUG: doc comments are ignored
// BUG: associated constants are not supported
// BUG: rename `lit` -> `literal`
// BUG: probably inner attributes are allowed in many more places
// BUG: refactor `use_path` syntax to be like `path`, remove `any_ident`
// BUG: `let [a, xs.., d] = out;` does not parse
// BUG: ambiguity between expression macros, stmt macros, item macros
