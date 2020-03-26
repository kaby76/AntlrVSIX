parser grammar FlexParser;

options { tokenVocab=FlexLexer; }

goal  : initlex sect1 sect1end sect2 initforrule
  ;
initlex  :
  ;
sect1  : sect1 startconddecl namelist1
  | sect1 options_nonterminal
  |
  | error
  ;
sect1end  : SECTEND
  ;
startconddecl  : SCDECL
  | XSCDECL
  ;
namelist1  : namelist1 NAME
  | NAME
  | error
  ;
options_nonterminal  : TOK_OPTION optionlist
  ;
optionlist  : optionlist option
  |
  ;
option  : TOK_OUTFILE EQUAL NAME
  | TOK_EXTRA_TYPE EQUAL NAME
  | TOK_PREFIX EQUAL NAME
  | TOK_YYCLASS EQUAL NAME
  | TOK_HEADER_FILE EQUAL NAME
  | TOK_TABLES_FILE EQUAL NAME
  ;
sect2  : sect2 scon initforrule flexrule NL
  | sect2 scon OPEN_CURLY sect2 CLOSE_CURLY
  |
  ;
initforrule  :
  ;
flexrule  : UP rule_nonterminal
  | rule_nonterminal
  | EOF_OP
  | error
  ;
scon_stk_ptr  :
  ;
scon  : LT scon_stk_ptr namelist2 GT
  | LT STAR GT
  |
  ;
namelist2  : namelist2 COMMA sconname
  | sconname
  | error
  ;
sconname  : NAME
  ;
rule_nonterminal  : re2 re
  | re2 re DOLLAR
  | re DOLLAR
  | re
  ;
re  : re VBAR series
  | series
  ;
re2  : re SLASH
  ;
series  : series singleton
  | singleton
  | series BEGIN_REPEAT_POSIX NUMBER COMMA NUMBER END_REPEAT_POSIX
  | series BEGIN_REPEAT_POSIX NUMBER COMMA END_REPEAT_POSIX
  | series BEGIN_REPEAT_POSIX NUMBER END_REPEAT_POSIX
  ;
singleton  : singleton STAR
  | singleton PLUS
  | singleton QUESTION
  | singleton BEGIN_REPEAT_FLEX NUMBER COMMA NUMBER END_REPEAT_FLEX
  | singleton BEGIN_REPEAT_FLEX NUMBER COMMA END_REPEAT_FLEX
  | singleton BEGIN_REPEAT_FLEX NUMBER END_REPEAT_FLEX
  | DOT
  | fullccl
  | PREVCCL
  | DQUOTE string DQUOTE
  | OPEN_PAREN re CLOSE_PAREN
  | CHAR
  ;
fullccl  : fullccl CCL_OP_DIFF braceccl
  | fullccl CCL_OP_UNION braceccl
  | braceccl
  ;
braceccl  : OPEN_BRACKET ccl CLOSE_BRACKET
  | OPEN_BRACKET UP ccl CLOSE_BRACKET
  ;
ccl  : ccl CHAR MINUS CHAR
  | ccl CHAR
  | ccl ccl_expr
  |
  ;
ccl_expr  : CCE_ALNUM
  | CCE_ALPHA
  | CCE_BLANK
  | CCE_CNTRL
  | CCE_DIGIT
  | CCE_GRAPH
  | CCE_LOWER
  | CCE_PRINT
  | CCE_PUNCT
  | CCE_SPACE
  | CCE_XDIGIT
  | CCE_UPPER
  | CCE_NEG_ALNUM
  | CCE_NEG_ALPHA
  | CCE_NEG_BLANK
  | CCE_NEG_CNTRL
  | CCE_NEG_DIGIT
  | CCE_NEG_GRAPH
  | CCE_NEG_PRINT
  | CCE_NEG_PUNCT
  | CCE_NEG_SPACE
  | CCE_NEG_XDIGIT
  | CCE_NEG_LOWER
  | CCE_NEG_UPPER
  ;
string  : string CHAR
  |
  ;
error : ERROR ;


