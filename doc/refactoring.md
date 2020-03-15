# Refactorings for Antlr Grammars

Antlrvsix implements a number of refactoring transformations
for Antlr grammars. These refactorings can be used to help
refine your grammar to make it more readable and more efficient.

## Replace Literals in Parser with Lexer Token Symbols

This transformation takes a parser or combined grammar
and converts the string literals in the parser rules to
lexer token symbols. In fact, the Antlr runtime performs
this mapping. However, some people may feel using the string
literal instead of the lexer token symbol clouds the readability
of the grammar since you don't know exactly which token the
literal corresponds to in the lexer/parser. While the
[documentation](https://github.com/antlr/antlr4/blob/master/doc/lexer-rules.md)
for Antlr says the tool does not allow string literals in a
parser grammar, it appears it does at least with Antlr 4.8,
so Antlrvsix does not enforce this when splitting a combined
grammar.

### Example

_Before_

    grammar Expression;
    e : e ('** | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+') a
      ;
    a : INT ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

_After_

    grammar Expression;
    e : e (MUL | DIV) e
      | e (ADD | SUB) e
      | LP e RP
      | (SUB | ADD) a
      ;
    a : INT ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    MIN : '-' ;
    WS : [ \r\n\t] + -> skip ;

## Remove Useless Productions

This transformation takes a parser or combined grammar,
and C# source, and removes all parse rules that are not used.
The C# source is parsed via Microsoft's Rosyln analysis tool
in order to find all calls to a parser rule of the form
"parser.rule()" where "rule" is the name of the parser rule.
If there is no source, the refactoring simply looks only at the
grammar. Lexer rules are currently not removed.

### Example

_Before_

    grammar Expression;
    e : e ('** | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+') a
      ;
    a : INT ;
    id : ID ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    ID : ( ('a' .. 'z') | ('A' .. 'Z') | '_' )+ ;
    WS : [ \r\n\t] + -> skip ;

_After_

    grammar Expression;
    e : e ('** | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+') a
      ;
    a : INT ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    ID : ( ('a' .. 'z') | ('A' .. 'Z') | '_' )+ ;
    WS : [ \r\n\t] + -> skip ;
