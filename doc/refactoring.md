# Refactorings for Antlr Grammars

Antlrvsix implements a number of refactoring transformations
for Antlr grammars. These refactorings can be used to help
refine your grammar to make it more readable and more efficient.

## Replace Literals in Parser with Lexer Token Symbols

This transformation takes a parser or combined grammar
and converts the string literals in the parser rules to
lexer token symbols. In fact, the Antlr tool performs
this mapping. However, some people may feel using the string
literal instead of the lexer token symbol clouds the readability
of the grammar since you don't know exactly which token the
literal corresponds to in the lexer/parser. While the
[documentation](https://github.com/antlr/antlr4/blob/master/doc/lexer-rules.md)
for Antlr says the tool does not allow string literals in a
parser grammar, it appears it does allow string literals in
parser grammars at least with Antlr 4.8,
so Antlrvsix does not enforce this when splitting a combined
grammar.

Note: This refactoring is a "fold" transformation
as described in Halupka, Ivan, and Ján Kollár. "Catalog of grammar refactoring patterns." Open Computer Science 4.4 (2014): 231-241.
However, this refactoring only applies to lexer symbols.

### Example

_Before_

    grammar Expression;
    e : e ('*' | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+')* a
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
      | (SUB | ADD)* a
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
    e : e ('*' | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+')* a
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
    e : e ('*' | '/') e
      | e ('+' | '-') e
      | '(' e ')'
      | ('-' | '+')* a
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

## Reorder parser rules

There are three different refactorings to reorder parser
rules in a parser or combined grammar: alphabetic, dfs, bfs.
For BFS or DFS orderings, Antlrvsix will examine the C#
source code to determine the start symbols for the grammar.
Reordering never applies to lexer rules, as recognition by these
rules is ordered. For combined grammars, the lexer rules are placed
at the end of the grammar file. Formatting of the rules is
perserved but formatting between rules may not. Use "reformat"
to reformat the grammars to your style.

_Before_

    grammar Expression;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    a : number | variable ;
    number : INT ;
    variable : VARIABLE ;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

_Alphabetic After_

    grammar Expression;
    a : number | variable ;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    number : INT ;
    variable : VARIABLE ;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

_DFS After_

    grammar Expression;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    a : number | variable ;
    number : INT ;
    variable : VARIABLE ;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

_BFS After_

    grammar Expression;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    a : number | variable ;
    number : INT ;
    variable : VARIABLE ;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

## Splitting and combining grammars

Antlrvsix provides two refactorings for splitting or combining
grammars. These refactorings are useful when
you need to introduce modes in lexers, or when you want to
have one grammar for a language.

Splitting a grammar converts a combined grammar into
split parser and lexer grammars. Arguments to the Antlr
tool are applied to the resulting grammars.

Notes:
* When splitting grammars, "option { tokenVocab=....; }" is inserted
into the parser. When combining grammars, the tokenVocab option is removed.
If there are no other options in the options spec, then the entire option
is removed.
* When splitting or combining, the generated Antlr listeners
and visitors are renamed. The refactoring does not currently
replace those references.
* When splitting, string literals that do not have a lexer symbol declaration
for folding will be left alone. You cannot use a string literal without a lexer
symbol declaration in a split grammar, but you can for a combined grammar.

_Before_

Combined grammar:

    grammar Expression;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    a : number | variable ;
    number : INT ;
    variable : VARIABLE ;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

_After_

Lexer grammar:

    lexer grammar ExpressionLexer;
    VARIABLE : VALID_ID_START VALID_ID_CHAR* ;
    fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
    fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
    INT : ('0' .. '9')+ ;
    MUL : '*' ;
    DIV : '/' ;
    ADD : '+' ;
    SUB : '-' ;
    LP : '(' ;
    RP : ')' ;
    WS : [ \r\n\t] + -> skip ;

Parser grammar:

    parser grammar ExpressionParser;
    e : e ('*' | '/') e
     | e ('+' | '-') e
     | '(' e ')'
     | ('-' | '+')* a
     ;
    a : number | variable ;
    number : INT ;
    variable : VARIABLE ;

