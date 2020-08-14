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
Because Antlr does not have a start symbol, which one
would find in a formal grammar, nor syntax to
declare a start symbol, 
the C# source is parsed via Microsoft's Rosyln analysis tool
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
Reordering never applies to lexer rules, since this is for parser
rules only, but also because the lexer rules are partially ordered (into modes).
For combined grammars, the lexer rules are placed
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
into the parser grammar. When combining grammars, the tokenVocab option is removed
from the combined grammar.
If there are no other options in the options spec, then the entire option
is removed.
* When splitting or combining, the generated Antlr listeners
and visitors are going to be named differently. The refactoring does not currently
replace those references in your C# code.
* When splitting, string literals that do not have a lexer symbol declaration
for folding will be left alone, and may result in build errors by the Antlr tool.
You can use a string literal without a lexer
symbol declaration in a combined grammar, but you cannot for a split grammar.

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

## Unfold

Unfold replaces the applied occurrence of a lexer or parser rule
symbol with the right-hand side of the rule. This transformation
is useful for situations where you want to reduce the parse tree
height, which helps to reduce the size of the parse tree. The unfold
operation takes a parse tree and a collection of leaf nodes in the parse
tree corresponding to either an applied occurrence of a symbol to replace,
or a defining occurrence of a symbol that sets all applied occurrences
to be replaced.

Example:

Suppose we want to unfold the rule `e` that occurs in rule `s`. After executing
`unfold "//parserRuleSpec[RULE_REF/text() = 's']//labeledAlt//RULE_REF[text() = 'e']"`
the stack now contains this file, which is not parsed:

_Before_

    grammar A;
    s : e ;
    e : e '*' e       # Mult
        | INT           # primary
        ;
    INT : [0-9]+ ;
    WS : [ \t\n]+ -> skip ;

_After_

    grammar A;
    s : ( e '*' e | INT ) ;
    e : e '*' e           # Mult
        | INT               # primary
        ;
    INT : [0-9]+ ;
    WS : [ \t\n]+ -> skip ;

## Remove useless parentheses

There are times when a rule contains parentheses that surround a single symbol
or list of symbols. In some situations, these parentheses can be removed without
changing the meaning of the rule. This transformation perform this refactoring.

### Example

_Before_

    grammar A2;
    s : e ;
    e : e '*' ( e ) | int ;
    int : INT ;
    INT : [0-9]+ ;
    WS : [ \t\n]+ -> skip ;

_Trash command_

`rup "//parserRuleSpec[RULE_REF/text() = 'e']//labeledAlt//block"`

_After_

    grammar A2;
    s : e ;
    e : e '*' e | int ;
    int : INT ;
    INT : [0-9]+ ;
    WS : [ \t\n]+ -> skip ;

## Upper and lower case string literals

In Antlr2, there was an option (`caseSensitive`) to match upper and lower case
string literals for keywords. In order to migrate to Antlr4, the `ulliteral`
transform was created. In order for the lexer to recognize keywords in any
case, the string literal for the keyword must be replaced with a sequence
of upper and lower case sets. See this [explanation](https://theantlrguy.atlassian.net/wiki/spaces/ANTLR3/pages/2687342/How+do+I+get+case+insensitivity).

### Example

_Before_

    grammar KeywordFun;
    a : 'abc';
    b : 'def';
    A: 'abc';
    B: 'def';
    C: 'uvw' 'xyz'?;
    D: 'uvw' 'xyz'+;

_Trash command_

    ulliteral "//lexerRuleSpec[TOKEN_REF/text() = 'A']//STRING_LITERAL"
    ulliteral "//lexerRuleSpec[TOKEN_REF/text() = 'B']//STRING_LITERAL"

_After_

    grammar KeywordFun;
    a : 'abc';
    b : 'def';
    A: [aA] [bB] [cC];
    B: [dD] [eE] [fF];
    C: 'uvw' 'xyz'?;
    D: 'uvw' 'xyz'+;

### Delete parse tree node

For whatever reason, sometimes you just want to delete a collection of nodes.
This transformation
takes a parse tree node, and deletes it from the parse tree.

_Before_

    grammar KeywordFun;
    a : 'abc';
    b : 'def';
    A: 'abc';
    B: 'def';
    C: 'uvw' 'xyz'?;
    D: 'uvw' 'xyz'+;

_Trash command_

    delete "//lexerRuleSpec[TOKEN_REF/text() = 'A']"

_After_

    grammar KeywordFun;
    a : 'abc';
    b : 'def';
    B: 'def';
    C: 'uvw' 'xyz'?;
    D: 'uvw' 'xyz'+;


### Unify alts to EBNF

After unfolding rules, you will probably notice some `ruleAltList` or other
alt lists that have a common left factor. The Unify transform is a
generalization of the left factoring rule. This powerful transform performs
a N-way merge of all the alts into one sequence with ebnf blocks
created for non-common elements.

_Before_

    grammar CommonFactors;
    a : 'X' 'B' 'Z' | 'X' 'C' 'Z' | 'X' 'D' 'Z' ;

_Trash command_

    unify "//parserRuleSpec[RULE_REF/text() = 'a']//ruleAltList"

_After_

    grammar CommonFactors;
    a : 'X' ( 'B' | 'C' | 'D' ) 'Z' ;
