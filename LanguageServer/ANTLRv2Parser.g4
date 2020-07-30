/*
 [The "BSD licence"]
 Copyright (c) 2005-2007 Terence Parr
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions
 are met:
 1. Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.
 2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.
 3. The name of the author may not be used to endorse or promote products
    derived from this software without specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
parser grammar ANTLRv2Parser;

options
{
    tokenVocab = ANTLRv2Lexer;
    contextSuperClass=AntlrTreeEditing.AntlrDOM.ObserverParserRuleContext;
}

grammar_
   : ( HEADER STRING_LITERAL? action )* fileOptionsSpec? classDef* EOF
   ;

classDef
   : action? DOC_COMMENT? ( lexerSpec | treeParserSpec | parserSpec ) rules
   ;
   
fileOptionsSpec
   : OPTIONS LBRACE option* RBRACE
   ;

parserOptionsSpec
   : OPTIONS LBRACE option* RBRACE
   ;

treeParserOptionsSpec
   : OPTIONS LBRACE option* RBRACE
   ;

lexerOptionsSpec
   : OPTIONS LBRACE lexerOption* RBRACE
   ;

subruleOptionsSpec
   : OPTIONS LBRACE option* RBRACE
   ;

option
   : id EQUAL optionValue SEMI
   ;

optionValue
   : qualifiedID
   | STRING_LITERAL
   | CHAR_LITERAL
   | INT
   ;

lexerOption
   : id EQUAL lexerOptionValue SEMI
   ;

lexerOptionValue
   : charSet
   | optionValue
   ;

charSet
   : setBlockElement ( OR setBlockElement)*
   ;

setBlockElement
   : CHAR_LITERAL (RANGE CHAR_LITERAL)?
   ;

tokensSpec
   : TOKENS LBRACE tokenSpec+ RBRACE
   ;

tokenSpec
   : (
       TOKEN_REF ( EQUAL (STRING_LITERAL | CHAR_LITERAL) | )
       | STRING_LITERAL tokensSpecOptions
     )
     SEMI
   ;

tokensSpecOptions
   : OPEN_ELEMENT_OPITON id ASSIGN optionValue ( SEMI id ASIGN optionValue )* CLOSE_ELEMENT_OPTION
   ;

superClass
   : LPAREN STRING_LITERAL RPAREN
   ;

parserSpec
   : CLASS id (EXTENDS PARSER superClass? | ) SEMI parserOptionsSpec? tokensSpec? action?
   ;

lexerSpec
   : (LEXCLASS id | CLASS id EXTENDS LEXER superClass?)
   SEMI lexerOptionsSpec? tokensSpec? action?
   ;

treeParserSpec
   : CLASS id EXTENDS TREEPARSER superClass? SEMI treeParserOptionsSpec? tokensSpec? action?
   ;

rules
   : rule_+
   ;

rule_
   : DOC_COMMENT? ((PROTECTED | PUBLIC | PRIVATE))? id BANG? argActionBlock? (RETURNS argActionBlock)? throwsSpec? ruleOptionsSpec? ruleAction* COLON altList SEMI exceptionGroup?
   ;

ruleOptionsSpec
   : OPTIONS LBRACE option* RBRACE
   ;

throwsSpec
   : THROWS id (COMMA id)*
   ;

block
   : alternative (OR alternative )*
   ;

alternative
   : BANG? element*
   ;

exceptionGroup
   : exceptionSpec+
   ;

exceptionSpec
   : EXCEPTION 
   // TODO ARG_ACTION
   exceptionHandler*
   ;

exceptionSpecNoLabel
   : EXCEPTION
   exceptionHandler*
   ;

exceptionHandler
   : CATCH argActionBlock actionBlock
   ;

element
   : elementNoOptionSpec elementOptionSpec?
   ;

elementOptionSpec
   : OPEN_ELEMENT_OPTION
     id ASSIGN optionValue
     ( SEMI id ASSIGN optionValue )*
     CLOSE_ELEMENT_OPTION
     ;

elementNoOptionSpec
   : (id EQUAL (id COLON)? (RULE_REF argActionBlock? BANG? | TOKEN_REF argActionBlock?))
   | ((id COLON)? (RULE_REF argActionBlock? BANG? | range | terminal_ | NOT_OP ( notTerminal | ebnf) | ebnf))
   | actionBlock
   | SEMPRED
   | tree_
   ;
   
tree_
   : TREE_BEGIN rootNode element+ RPAREN
   ;

rootNode
   : (id COLON)? terminal_
   ;

ebnf
   : LPAREN ( subruleOptionsSpec COLON | actionBlock COLON)?
   block RPAREN
   ( (QM | STAR | PLUS)? BANG? | IMPLIES)
   ;

ast_type_spec
   : (CART | BANG)?
   ;

range
   : CHAR_LITERAL RANGE CHAR_LITERAL BANG?
   | (TOKEN_REF | STRING_LITERAL) RANGE ast_type_spec
   ;

terminal_
   : CHAR_LITERAL BANG?
   | TOKEN_REF ast_type_spec argActionBlock?
   | STRING_LITERAL ast_type_spec
   | DOT ast_type_spec
   ;

notTerminal
   : CHAR_LITERAL BANG?
   | TOKEN_REF ast_type_spec
   ;

qualifiedID
   : id ( DOT id)*
   ;

id
   : TOKEN_REF
   | RULE_REF
   ;


action
   : AT (actionScopeName COLONCOLON)? id actionBlock
   ;

actionScopeName
   : id
   | LEXER
   | PARSER
   ;

   
ruleAction
   : AT id actionBlock
   ;

altList
   : alternative (OR alternative )*
   ;

actionBlock
   : BEGIN_ACTION ACTION_CONTENT* END_ACTION
   ;

argActionBlock
   : BEGIN_ARGUMENT ARGUMENT_CONTENT* END_ARGUMENT
   ;

