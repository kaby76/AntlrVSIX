grammar Iso14977ebnf;

// https://www.iso.org/standard/26153.html
// https://dwheeler.com/essays/dont-use-iso-14977-ebnf.html
// https://www.grammarware.net/text/2012/bnf-was-here.pdf

RepSym : '*';
ExcSym : '-';
ConSym : ',';
DefSepSym : '|';
DefSym : '=';
TerSym : ';';

SqSym : '\'';
DqSym : '"';
StartCommentSym : '(*';
EndCommentSym : '*)';
StartGroupSym : '(';
EndGroupSym : ')';
StartOptSym : '[';
EndOptSym : ']';
StartRepeatSym : '{';
EndRepeatSym : '}';
SpecialSeqSym : '?';

SymbolKeyword : 'symbol';
Identifier : [a-zA-Z0-9][a-zA-Z0-9_\-];
