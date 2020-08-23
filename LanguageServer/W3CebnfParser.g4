
// https://www.w3.org/TR/REC-xml/#sec-notation

parser grammar W3CebnfParser;

options { tokenVocab = W3CebnfLexer; }

prods : prod+ EOF ;
prod : lhs PPEQ rhs ;
lhs : SYMBOL ;
rhs : alts ;
symbol : SYMBOL ;
alts : alt ( VP alt )* ;
alt : element* ;
element : block ( M block )*  ;
block : atom suffix? ;
atom : SYMBOL | SET | STRING | OP alts CP ;
suffix : Q Q? | S Q? | P Q? ;
