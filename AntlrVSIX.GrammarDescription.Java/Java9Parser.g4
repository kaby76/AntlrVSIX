/*
 * [The "BSD license"]
 *  Copyright (c) 2014 Terence Parr
 *  Copyright (c) 2014 Sam Harwell
 *  Copyright (c) 2017 Chan Chung Kwong
 *  All rights reserved.
 *
 *  Redistribution and use in source and binary forms, with or without
 *  modification, are permitted provided that the following conditions
 *  are met:
 *
 *  1. Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 *  2. Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 *  3. The name of the author may not be used to endorse or promote products
 *     derived from this software without specific prior written permission.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 *  IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 *  OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 *  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 *  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 *  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 *  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 *  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 *  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/**
 * A Java 8 grammar for ANTLR 4 derived from the Java Language Specification
 * chapter 19.
 *
 * NOTE: This grammar results in a generated parser that is much slower
 *       than the Java 7 grammar in the grammars-v4/java directory. This
 *     one is, however, extremely close to the spec.
 *
 * You can test with
 *
 *  $ antlr4 Java9.g4
 *  $ javac *.java
 *  $ grun Java9 compilationUnit *.java
 *
 * Or,
~/antlr/code/grammars-v4/java9 $ java Test .
/Users/parrt/antlr/code/grammars-v4/java9/./Java9BaseListener.java
/Users/parrt/antlr/code/grammars-v4/java9/./Java9Lexer.java
/Users/parrt/antlr/code/grammars-v4/java9/./Java9Listener.java
/Users/parrt/antlr/code/grammars-v4/java9/./Java9Parser.java
/Users/parrt/antlr/code/grammars-v4/java9/./Test.java
Total lexer+parser time 30844ms.
~/antlr/code/grammars-v4/java9 $ java Test examples/module-info.java
/home/kwong/projects/grammars-v4/java9/examples/module-info.java
Total lexer+parser time 914ms.
~/antlr/code/grammars-v4/java9 $ java Test examples/TryWithResourceDemo.java
/home/kwong/projects/grammars-v4/java9/examples/TryWithResourceDemo.java
Total lexer+parser time 3634ms.
~/antlr/code/grammars-v4/java9 $ java Test examples/helloworld.java
/home/kwong/projects/grammars-v4/java9/examples/helloworld.java
Total lexer+parser time 2497ms.

 */
parser grammar Java9Parser;

options {
	tokenVocab = Java9Lexer ;
}

/*
 * Productions from §3 (Lexical Structure)
 */

literal
	:	IntegerLiteral
	|	FloatingPointLiteral
	|	BooleanLiteral
	|	CharacterLiteral
	|	StringLiteral
	|	NullLiteral
	;

/*
 * Productions from §4 (Types, Values, and Variables)
 */

primitiveType
	:	annotation* numericType
	|	annotation* BOOLEAN
	;

numericType
	:	integralType
	|	floatingPointType
	;

integralType
	:	BYTE
	|	SHORT
	|	INT
	|	LONG
	|	CHAR
	;

floatingPointType
	:	FLOAT
	|	DOUBLE
	;

referenceType
	:	classOrInterfaceType
	|	typeVariable
	|	arrayType
	;

/*classOrInterfaceType
	:	classType
	|	interfaceType
	;
*/
classOrInterfaceType
	:	(	classType_lfno_classOrInterfaceType
		|	interfaceType_lfno_classOrInterfaceType
		)
		(	classType_lf_classOrInterfaceType
		|	interfaceType_lf_classOrInterfaceType
		)*
	;

classType
	:	annotation* identifier typeArguments?
	|	classOrInterfaceType DOT annotation* identifier typeArguments?
	;

classType_lf_classOrInterfaceType
	:	DOT annotation* identifier typeArguments?
	;

classType_lfno_classOrInterfaceType
	:	annotation* identifier typeArguments?
	;

interfaceType
	:	classType
	;

interfaceType_lf_classOrInterfaceType
	:	classType_lf_classOrInterfaceType
	;

interfaceType_lfno_classOrInterfaceType
	:	classType_lfno_classOrInterfaceType
	;

typeVariable
	:	annotation* identifier
	;

arrayType
	:	primitiveType dims
	|	classOrInterfaceType dims
	|	typeVariable dims
	;

dims
	:	annotation* LBRACK RBRACK (annotation* LBRACK RBRACK)*
	;

typeParameter
	:	typeParameterModifier* identifier typeBound?
	;

typeParameterModifier
	:	annotation
	;

typeBound
	:	EXTENDS typeVariable
	|	EXTENDS classOrInterfaceType additionalBound*
	;

additionalBound
	:	BITAND interfaceType
	;

typeArguments
	:	LT typeArgumentList GT
	;

typeArgumentList
	:	typeArgument (COMMA typeArgument)*
	;

typeArgument
	:	referenceType
	|	wildcard
	;

wildcard
	:	annotation* QUESTION wildcardBounds?
	;

wildcardBounds
	:	EXTENDS referenceType
	|	SUPER referenceType
	;

/*
 * Productions from §6 (Names)
 */

moduleName
	:	identifier
	|	moduleName DOT identifier
	;

packageName
	:	identifier
	|	packageName DOT identifier
	;

typeName
	:	identifier
	|	packageOrTypeName DOT identifier
	;

packageOrTypeName
	:	identifier
	|	packageOrTypeName DOT identifier
	;

expressionName
	:	identifier
	|	ambiguousName DOT identifier
	;

methodName
	:	identifier
	;

ambiguousName
	:	identifier
	|	ambiguousName DOT identifier
	;

/*
 * Productions from §7 (Packages)
 */

compilationUnit
	:	ordinaryCompilation
	|	modularCompilation
	;

ordinaryCompilation
	:	packageDeclaration? importDeclaration* typeDeclaration* EOF
	;

modularCompilation
	:	importDeclaration* moduleDeclaration
	;

packageDeclaration
	:	packageModifier* PACKAGE packageName SEMI
	;

packageModifier
	:	annotation
	;

importDeclaration
	:	singleTypeImportDeclaration
	|	typeImportOnDemandDeclaration
	|	singleStaticImportDeclaration
	|	staticImportOnDemandDeclaration
	;

singleTypeImportDeclaration
	:	IMPORT typeName SEMI
	;

typeImportOnDemandDeclaration
	:	IMPORT packageOrTypeName DOT MUL SEMI
	;

singleStaticImportDeclaration
	:	IMPORT STATIC typeName DOT identifier SEMI
	;

staticImportOnDemandDeclaration
	:	IMPORT STATIC typeName DOT MUL SEMI
	;

typeDeclaration
	:	classDeclaration
	|	interfaceDeclaration
	|	SEMI
	;

moduleDeclaration
	:	annotation* OPEN? MODULE moduleName LBRACE moduleDirective* RBRACE
	;

moduleDirective
	:	REQUIRES requiresModifier* moduleName SEMI
	|	EXPORTS packageName (TO moduleName (COMMA moduleName)*)? SEMI
	|	OPENS packageName (TO moduleName (COMMA moduleName)*)? SEMI
	|	USES typeName SEMI
	|	PROVIDES typeName WITH typeName (COMMA typeName)* SEMI
	;

requiresModifier
	:	TRANSITIVE
	|	STATIC
	;

/*
 * Productions from §8 (Classes)
 */

classDeclaration
	:	normalClassDeclaration
	|	enumDeclaration
	;

normalClassDeclaration
	:	classModifier* CLASS identifier typeParameters? superclass? superinterfaces? classBody
	;

classModifier
	:	annotation
	|	PUBLIC
	|	PROTECTED
	|	PRIVATE
	|	ABSTRACT
	|	STATIC
	|	FINAL
	|	STRICTFP
	;

typeParameters
	:	LT typeParameterList GT
	;

typeParameterList
	:	typeParameter (COMMA typeParameter)*
	;

superclass
	:	EXTENDS classType
	;

superinterfaces
	:	IMPLEMENTS interfaceTypeList
	;

interfaceTypeList
	:	interfaceType (COMMA interfaceType)*
	;

classBody
	:	LBRACE classBodyDeclaration* RBRACE
	;

classBodyDeclaration
	:	classMemberDeclaration
	|	instanceInitializer
	|	staticInitializer
	|	constructorDeclaration
	;

classMemberDeclaration
	:	fieldDeclaration
	|	methodDeclaration
	|	classDeclaration
	|	interfaceDeclaration
	|	SEMI
	;

fieldDeclaration
	:	fieldModifier* unannType variableDeclaratorList SEMI
	;

fieldModifier
	:	annotation
	|	PUBLIC
	|	PROTECTED
	|	PRIVATE
	|	STATIC
	|	FINAL
	|	TRANSIENT
	|	VOLATILE
	;

variableDeclaratorList
	:	variableDeclarator (COMMA variableDeclarator)*
	;

variableDeclarator
	:	variableDeclaratorId (ASSIGN variableInitializer)?
	;

variableDeclaratorId
	:	identifier dims?
	;

variableInitializer
	:	expression
	|	arrayInitializer
	;

unannType
	:	unannPrimitiveType
	|	unannReferenceType
	;

unannPrimitiveType
	:	numericType
	|	BOOLEAN
	;

unannReferenceType
	:	unannClassOrInterfaceType
	|	unannTypeVariable
	|	unannArrayType
	;

/*unannClassOrInterfaceType
	:	unannClassType
	|	unannInterfaceType
	;
*/

unannClassOrInterfaceType
	:	(	unannClassType_lfno_unannClassOrInterfaceType
		|	unannInterfaceType_lfno_unannClassOrInterfaceType
		)
		(	unannClassType_lf_unannClassOrInterfaceType
		|	unannInterfaceType_lf_unannClassOrInterfaceType
		)*
	;

unannClassType
	:	identifier typeArguments?
	|	unannClassOrInterfaceType DOT annotation* identifier typeArguments?
	;

unannClassType_lf_unannClassOrInterfaceType
	:	DOT annotation* identifier typeArguments?
	;

unannClassType_lfno_unannClassOrInterfaceType
	:	identifier typeArguments?
	;

unannInterfaceType
	:	unannClassType
	;

unannInterfaceType_lf_unannClassOrInterfaceType
	:	unannClassType_lf_unannClassOrInterfaceType
	;

unannInterfaceType_lfno_unannClassOrInterfaceType
	:	unannClassType_lfno_unannClassOrInterfaceType
	;

unannTypeVariable
	:	identifier
	;

unannArrayType
	:	unannPrimitiveType dims
	|	unannClassOrInterfaceType dims
	|	unannTypeVariable dims
	;

methodDeclaration
	:	methodModifier* methodHeader methodBody
	;

methodModifier
	:	annotation
	|	PUBLIC
	|	PROTECTED
	|	PRIVATE
	|	ABSTRACT
	|	STATIC
	|	FINAL
	|	SYNCHRONIZED
	|	NATIVE
	|	STRICTFP
	;

methodHeader
	:	result methodDeclarator throws_?
	|	typeParameters annotation* result methodDeclarator throws_?
	;

result
	:	unannType
	|	VOID
	;

methodDeclarator
	:	identifier LPAREN formalParameterList? RPAREN dims?
	;

formalParameterList
	:	formalParameters COMMA lastFormalParameter
	|	lastFormalParameter
	|	receiverParameter
	;

formalParameters
	:	formalParameter (COMMA formalParameter)*
	|	receiverParameter (COMMA formalParameter)*
	;

formalParameter
	:	variableModifier* unannType variableDeclaratorId
	;

variableModifier
	:	annotation
	|	FINAL
	;

lastFormalParameter
	:	variableModifier* unannType annotation* ELLIPSIS variableDeclaratorId
	|	formalParameter
	;

receiverParameter
	:	annotation* unannType (identifier DOT)? THIS
	;

throws_
	:	THROWS exceptionTypeList
	;

exceptionTypeList
	:	exceptionType (COMMA exceptionType)*
	;

exceptionType
	:	classType
	|	typeVariable
	;

methodBody
	:	block
	|	SEMI
	;

instanceInitializer
	:	block
	;

staticInitializer
	:	STATIC block
	;

constructorDeclaration
	:	constructorModifier* constructorDeclarator throws_? constructorBody
	;

constructorModifier
	:	annotation
	|	PUBLIC
	|	PROTECTED
	|	PRIVATE
	;

constructorDeclarator
	:	typeParameters? simpleTypeName LPAREN formalParameterList? RPAREN
	;

simpleTypeName
	:	identifier
	;

constructorBody
	:	LBRACE explicitConstructorInvocation? blockStatements? RBRACE
	;

explicitConstructorInvocation
	:	typeArguments? THIS LPAREN argumentList? RPAREN SEMI
	|	typeArguments? SUPER LPAREN argumentList? RPAREN SEMI
	|	expressionName DOT typeArguments? SUPER LPAREN argumentList? RPAREN SEMI
	|	primary DOT typeArguments? SUPER LPAREN argumentList? RPAREN SEMI
	;

enumDeclaration
	:	classModifier* ENUM identifier superinterfaces? enumBody
	;

enumBody
	:	LBRACE enumConstantList? COMMA? enumBodyDeclarations? RBRACE
	;

enumConstantList
	:	enumConstant (COMMA enumConstant)*
	;

enumConstant
	:	enumConstantModifier* identifier (LPAREN argumentList? RPAREN)? classBody?
	;

enumConstantModifier
	:	annotation
	;

enumBodyDeclarations
	:	SEMI classBodyDeclaration*
	;

/*
 * Productions from §9 (Interfaces)
 */

interfaceDeclaration
	:	normalInterfaceDeclaration
	|	annotationTypeDeclaration
	;

normalInterfaceDeclaration
	:	interfaceModifier* INTERFACE identifier typeParameters? extendsInterfaces? interfaceBody
	;

interfaceModifier
	:	annotation
	|	PUBLIC
	|	PROTECTED
	|	PRIVATE
	|	ABSTRACT
	|	STATIC
	|	STRICTFP
	;

extendsInterfaces
	:	EXTENDS interfaceTypeList
	;

interfaceBody
	:	LBRACE interfaceMemberDeclaration* RBRACE
	;

interfaceMemberDeclaration
	:	constantDeclaration
	|	interfaceMethodDeclaration
	|	classDeclaration
	|	interfaceDeclaration
	|	SEMI
	;

constantDeclaration
	:	constantModifier* unannType variableDeclaratorList SEMI
	;

constantModifier
	:	annotation
	|	PUBLIC
	|	STATIC
	|	FINAL
	;

interfaceMethodDeclaration
	:	interfaceMethodModifier* methodHeader methodBody
	;

interfaceMethodModifier
	:	annotation
	|	PUBLIC
	|	PRIVATE//Introduced in Java 9
	|	ABSTRACT
	|	DEFAULT
	|	STATIC
	|	STRICTFP
	;

annotationTypeDeclaration
	:	interfaceModifier* AT INTERFACE identifier annotationTypeBody
	;

annotationTypeBody
	:	LBRACE annotationTypeMemberDeclaration* RBRACE
	;

annotationTypeMemberDeclaration
	:	annotationTypeElementDeclaration
	|	constantDeclaration
	|	classDeclaration
	|	interfaceDeclaration
	|	SEMI
	;

annotationTypeElementDeclaration
	:	annotationTypeElementModifier* unannType identifier LPAREN RPAREN dims? defaultValue? SEMI
	;

annotationTypeElementModifier
	:	annotation
	|	PUBLIC
	|	ABSTRACT
	;

defaultValue
	:	DEFAULT elementValue
	;

annotation
	:	normalAnnotation
	|	markerAnnotation
	|	singleElementAnnotation
	;

normalAnnotation
	:	AT typeName LPAREN elementValuePairList? RPAREN
	;

elementValuePairList
	:	elementValuePair (COMMA elementValuePair)*
	;

elementValuePair
	:	identifier ASSIGN elementValue
	;

elementValue
	:	conditionalExpression
	|	elementValueArrayInitializer
	|	annotation
	;

elementValueArrayInitializer
	:	LBRACE elementValueList? COMMA? RBRACE
	;

elementValueList
	:	elementValue (COMMA elementValue)*
	;

markerAnnotation
	:	AT typeName
	;

singleElementAnnotation
	:	AT typeName LPAREN elementValue RPAREN
	;

/*
 * Productions from §10 (Arrays)
 */

arrayInitializer
	:	LBRACE variableInitializerList? COMMA? RBRACE
	;

variableInitializerList
	:	variableInitializer (COMMA variableInitializer)*
	;

/*
 * Productions from §14 (Blocks and Statements)
 */

block
	:	LBRACE blockStatements? RBRACE
	;

blockStatements
	:	blockStatement+
	;

blockStatement
	:	localVariableDeclarationStatement
	|	classDeclaration
	|	statement
	;

localVariableDeclarationStatement
	:	localVariableDeclaration SEMI
	;

localVariableDeclaration
	:	variableModifier* unannType variableDeclaratorList
	;

statement
	:	statementWithoutTrailingSubstatement
	|	labeledStatement
	|	ifThenStatement
	|	ifThenElseStatement
	|	whileStatement
	|	forStatement
	;

statementNoShortIf
	:	statementWithoutTrailingSubstatement
	|	labeledStatementNoShortIf
	|	ifThenElseStatementNoShortIf
	|	whileStatementNoShortIf
	|	forStatementNoShortIf
	;

statementWithoutTrailingSubstatement
	:	block
	|	emptyStatement
	|	expressionStatement
	|	assertStatement
	|	switchStatement
	|	doStatement
	|	breakStatement
	|	continueStatement
	|	returnStatement
	|	synchronizedStatement
	|	throwStatement
	|	tryStatement
	;

emptyStatement
	:	SEMI
	;

labeledStatement
	:	identifier COLON statement
	;

labeledStatementNoShortIf
	:	identifier COLON statementNoShortIf
	;

expressionStatement
	:	statementExpression SEMI
	;

statementExpression
	:	assignment
	|	preIncrementExpression
	|	preDecrementExpression
	|	postIncrementExpression
	|	postDecrementExpression
	|	methodInvocation
	|	classInstanceCreationExpression
	;

ifThenStatement
	:	IF LPAREN expression RPAREN statement
	;

ifThenElseStatement
	:	IF LPAREN expression RPAREN statementNoShortIf ELSE statement
	;

ifThenElseStatementNoShortIf
	:	IF LPAREN expression RPAREN statementNoShortIf ELSE statementNoShortIf
	;

assertStatement
	:	ASSERT expression SEMI
	|	ASSERT expression COLON expression SEMI
	;

switchStatement
	:	SWITCH LPAREN expression RPAREN switchBlock
	;

switchBlock
	:	LBRACE switchBlockStatementGroup* switchLabel* RBRACE
	;

switchBlockStatementGroup
	:	switchLabels blockStatements
	;

switchLabels
	:	switchLabel+
	;

switchLabel
	:	CASE constantExpression COLON
	|	CASE enumConstantName COLON
	|	DEFAULT COLON
	;

enumConstantName
	:	identifier
	;

whileStatement
	:	WHILE LPAREN expression RPAREN statement
	;

whileStatementNoShortIf
	:	WHILE LPAREN expression RPAREN statementNoShortIf
	;

doStatement
	:	DO statement WHILE LPAREN expression RPAREN SEMI
	;

forStatement
	:	basicForStatement
	|	enhancedForStatement
	;

forStatementNoShortIf
	:	basicForStatementNoShortIf
	|	enhancedForStatementNoShortIf
	;

basicForStatement
	:	FOR LPAREN forInit? SEMI expression? SEMI forUpdate? RPAREN statement
	;

basicForStatementNoShortIf
	:	FOR LPAREN forInit? SEMI expression? SEMI forUpdate? RPAREN statementNoShortIf
	;

forInit
	:	statementExpressionList
	|	localVariableDeclaration
	;

forUpdate
	:	statementExpressionList
	;

statementExpressionList
	:	statementExpression (COMMA statementExpression)*
	;

enhancedForStatement
	:	FOR LPAREN variableModifier* unannType variableDeclaratorId COLON expression RPAREN statement
	;

enhancedForStatementNoShortIf
	:	FOR LPAREN variableModifier* unannType variableDeclaratorId COLON expression RPAREN statementNoShortIf
	;

breakStatement
	:	BREAK identifier? SEMI
	;

continueStatement
	:	CONTINUE identifier? SEMI
	;

returnStatement
	:	RETURN expression? SEMI
	;

throwStatement
	:	THROW expression SEMI
	;

synchronizedStatement
	:	SYNCHRONIZED LPAREN expression RPAREN block
	;

tryStatement
	:	TRY block catches
	|	TRY block catches? finally_
	|	tryWithResourcesStatement
	;

catches
	:	catchClause+
	;

catchClause
	:	CATCH LPAREN catchFormalParameter RPAREN block
	;

catchFormalParameter
	:	variableModifier* catchType variableDeclaratorId
	;

catchType
	:	unannClassType (BITOR classType)*
	;

finally_
	:	FINALLY block
	;

tryWithResourcesStatement
	:	TRY resourceSpecification block catches? finally_?
	;

resourceSpecification
	:	LPAREN resourceList SEMI? RPAREN
	;

resourceList
	:	resource (SEMI resource)*
	;

resource
	:	variableModifier* unannType variableDeclaratorId ASSIGN expression
	|	variableAccess//Introduced in Java 9
	;

variableAccess
	:	expressionName
	|	fieldAccess
	;

/*
 * Productions from §15 (Expressions)
 */

/*primary
	:	primaryNoNewArray
	|	arrayCreationExpression
	;
*/

primary
	:	(	primaryNoNewArray_lfno_primary
		|	arrayCreationExpression
		)
		(	primaryNoNewArray_lf_primary
		)*
	;

primaryNoNewArray
	:	literal
	|	classLiteral
	|	THIS
	|	typeName DOT THIS
	|	LPAREN expression RPAREN
	|	classInstanceCreationExpression
	|	fieldAccess
	|	arrayAccess
	|	methodInvocation
	|	methodReference
	;

primaryNoNewArray_lf_arrayAccess
	:
	;

primaryNoNewArray_lfno_arrayAccess
	:	literal
	|	typeName (LBRACK RBRACK)* DOT CLASS
	|	VOID DOT CLASS
	|	THIS
	|	typeName DOT THIS
	|	LPAREN expression RPAREN
	|	classInstanceCreationExpression
	|	fieldAccess
	|	methodInvocation
	|	methodReference
	;

primaryNoNewArray_lf_primary
	:	classInstanceCreationExpression_lf_primary
	|	fieldAccess_lf_primary
	|	arrayAccess_lf_primary
	|	methodInvocation_lf_primary
	|	methodReference_lf_primary
	;

primaryNoNewArray_lf_primary_lf_arrayAccess_lf_primary
	:
	;

primaryNoNewArray_lf_primary_lfno_arrayAccess_lf_primary
	:	classInstanceCreationExpression_lf_primary
	|	fieldAccess_lf_primary
	|	methodInvocation_lf_primary
	|	methodReference_lf_primary
	;

primaryNoNewArray_lfno_primary
	:	literal
	|	typeName (LBRACK RBRACK)* DOT CLASS
	|	unannPrimitiveType (LBRACK RBRACK)* DOT CLASS
	|	VOID DOT CLASS
	|	THIS
	|	typeName DOT THIS
	|	LPAREN expression RPAREN
	|	classInstanceCreationExpression_lfno_primary
	|	fieldAccess_lfno_primary
	|	arrayAccess_lfno_primary
	|	methodInvocation_lfno_primary
	|	methodReference_lfno_primary
	;

primaryNoNewArray_lfno_primary_lf_arrayAccess_lfno_primary
	:
	;

primaryNoNewArray_lfno_primary_lfno_arrayAccess_lfno_primary
	:	literal
	|	typeName (LBRACK RBRACK)* DOT CLASS
	|	unannPrimitiveType (LBRACK RBRACK)* DOT CLASS
	|	VOID DOT CLASS
	|	THIS
	|	typeName DOT THIS
	|	LPAREN expression RPAREN
	|	classInstanceCreationExpression_lfno_primary
	|	fieldAccess_lfno_primary
	|	methodInvocation_lfno_primary
	|	methodReference_lfno_primary
	;

classLiteral
	:	(typeName|numericType|BOOLEAN) (LBRACK RBRACK)* DOT CLASS
	|	VOID DOT CLASS
	;

classInstanceCreationExpression
	:	NEW typeArguments? annotation* identifier (DOT annotation* identifier)* typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	|	expressionName DOT NEW typeArguments? annotation* identifier typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	|	primary DOT NEW typeArguments? annotation* identifier typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	;

classInstanceCreationExpression_lf_primary
	:	DOT NEW typeArguments? annotation* identifier typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	;

classInstanceCreationExpression_lfno_primary
	:	NEW typeArguments? annotation* identifier (DOT annotation* identifier)* typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	|	expressionName DOT NEW typeArguments? annotation* identifier typeArgumentsOrDiamond? LPAREN argumentList? RPAREN classBody?
	;

typeArgumentsOrDiamond
	:	typeArguments
	|	LT GT
	;

fieldAccess
	:	primary DOT identifier
	|	SUPER DOT identifier
	|	typeName DOT SUPER DOT identifier
	;

fieldAccess_lf_primary
	:	DOT identifier
	;

fieldAccess_lfno_primary
	:	SUPER DOT identifier
	|	typeName DOT SUPER DOT identifier
	;

/*arrayAccess
	:	expressionName LBRACK expression RBRACK
	|	primaryNoNewArray LBRACK expression RBRACK
	;
*/

arrayAccess
	:	(	expressionName LBRACK expression RBRACK
		|	primaryNoNewArray_lfno_arrayAccess LBRACK expression RBRACK
		)
		(	primaryNoNewArray_lf_arrayAccess LBRACK expression RBRACK
		)*
	;

arrayAccess_lf_primary
	:	(	primaryNoNewArray_lf_primary_lfno_arrayAccess_lf_primary LBRACK expression RBRACK
		)
		(	primaryNoNewArray_lf_primary_lf_arrayAccess_lf_primary LBRACK expression RBRACK
		)*
	;

arrayAccess_lfno_primary
	:	(	expressionName LBRACK expression RBRACK
		|	primaryNoNewArray_lfno_primary_lfno_arrayAccess_lfno_primary LBRACK expression RBRACK
		)
		(	primaryNoNewArray_lfno_primary_lf_arrayAccess_lfno_primary LBRACK expression RBRACK
		)*
	;


methodInvocation
	:	methodName LPAREN argumentList? RPAREN
	|	typeName DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	expressionName DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	primary DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	SUPER DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	typeName DOT SUPER DOT typeArguments? identifier LPAREN argumentList? RPAREN
	;

methodInvocation_lf_primary
	:	DOT typeArguments? identifier LPAREN argumentList? RPAREN
	;

methodInvocation_lfno_primary
	:	methodName LPAREN argumentList? RPAREN
	|	typeName DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	expressionName DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	SUPER DOT typeArguments? identifier LPAREN argumentList? RPAREN
	|	typeName DOT SUPER DOT typeArguments? identifier LPAREN argumentList? RPAREN
	;

argumentList
	:	expression (COMMA expression)*
	;

methodReference
	:	expressionName COLONCOLON typeArguments? identifier
	|	referenceType COLONCOLON typeArguments? identifier
	|	primary COLONCOLON typeArguments? identifier
	|	SUPER COLONCOLON typeArguments? identifier
	|	typeName DOT SUPER COLONCOLON typeArguments? identifier
	|	classType COLONCOLON typeArguments? NEW
	|	arrayType COLONCOLON NEW
	;

methodReference_lf_primary
	:	COLONCOLON typeArguments? identifier
	;

methodReference_lfno_primary
	:	expressionName COLONCOLON typeArguments? identifier
	|	referenceType COLONCOLON typeArguments? identifier
	|	SUPER COLONCOLON typeArguments? identifier
	|	typeName DOT SUPER COLONCOLON typeArguments? identifier
	|	classType COLONCOLON typeArguments? NEW
	|	arrayType COLONCOLON NEW
	;

arrayCreationExpression
	:	NEW primitiveType dimExprs dims?
	|	NEW classOrInterfaceType dimExprs dims?
	|	NEW primitiveType dims arrayInitializer
	|	NEW classOrInterfaceType dims arrayInitializer
	;

dimExprs
	:	dimExpr+
	;

dimExpr
	:	annotation* LBRACK expression RBRACK
	;

constantExpression
	:	expression
	;

expression
	:	lambdaExpression
	|	assignmentExpression
	;

lambdaExpression
	:	lambdaParameters ARROW lambdaBody
	;

lambdaParameters
	:	identifier
	|	LPAREN formalParameterList? RPAREN
	|	LPAREN inferredFormalParameterList RPAREN
	;

inferredFormalParameterList
	:	identifier (COMMA identifier)*
	;

lambdaBody
	:	expression
	|	block
	;

assignmentExpression
	:	conditionalExpression
	|	assignment
	;

assignment
	:	leftHandSide assignmentOperator expression
	;

leftHandSide
	:	expressionName
	|	fieldAccess
	|	arrayAccess
	;

assignmentOperator
	:	ASSIGN
	|	MUL_ASSIGN
	|	DIV_ASSIGN
	|	MOD_ASSIGN
	|	ADD_ASSIGN
	|	SUB_ASSIGN
	|	LSHIFT_ASSIGN
	|	RSHIFT_ASSIGN
	|	URSHIFT_ASSIGN
	|	AND_ASSIGN
	|	XOR_ASSIGN
	|	OR_ASSIGN
	;

conditionalExpression
	:	conditionalOrExpression
	|	conditionalOrExpression QUESTION expression COLON (conditionalExpression|lambdaExpression)
	;

conditionalOrExpression
	:	conditionalAndExpression
	|	conditionalOrExpression OR conditionalAndExpression
	;

conditionalAndExpression
	:	inclusiveOrExpression
	|	conditionalAndExpression AND inclusiveOrExpression
	;

inclusiveOrExpression
	:	exclusiveOrExpression
	|	inclusiveOrExpression BITOR exclusiveOrExpression
	;

exclusiveOrExpression
	:	andExpression
	|	exclusiveOrExpression CARET andExpression
	;

andExpression
	:	equalityExpression
	|	andExpression BITAND equalityExpression
	;

equalityExpression
	:	relationalExpression
	|	equalityExpression EQUAL relationalExpression
	|	equalityExpression NOTEQUAL relationalExpression
	;

relationalExpression
	:	shiftExpression
	|	relationalExpression LT shiftExpression
	|	relationalExpression GT shiftExpression
	|	relationalExpression LE shiftExpression
	|	relationalExpression GE shiftExpression
	|	relationalExpression INSTANCEOF referenceType
	;

shiftExpression
	:	additiveExpression
	|	shiftExpression LT LT additiveExpression
	|	shiftExpression GT GT additiveExpression
	|	shiftExpression GT GT GT additiveExpression
	;

additiveExpression
	:	multiplicativeExpression
	|	additiveExpression ADD multiplicativeExpression
	|	additiveExpression SUB multiplicativeExpression
	;

multiplicativeExpression
	:	unaryExpression
	|	multiplicativeExpression MUL unaryExpression
	|	multiplicativeExpression DIV unaryExpression
	|	multiplicativeExpression MOD unaryExpression
	;

unaryExpression
	:	preIncrementExpression
	|	preDecrementExpression
	|	ADD unaryExpression
	|	SUB unaryExpression
	|	unaryExpressionNotPlusMinus
	;

preIncrementExpression
	:	INC unaryExpression
	;

preDecrementExpression
	:	DEC unaryExpression
	;

unaryExpressionNotPlusMinus
	:	postfixExpression
	|	TILDE unaryExpression
	|	BANG unaryExpression
	|	castExpression
	;

/*postfixExpression
	:	primary
	|	expressionName
	|	postIncrementExpression
	|	postDecrementExpression
	;
*/

postfixExpression
	:	(	primary
		|	expressionName
		)
		(	postIncrementExpression_lf_postfixExpression
		|	postDecrementExpression_lf_postfixExpression
		)*
	;

postIncrementExpression
	:	postfixExpression INC
	;

postIncrementExpression_lf_postfixExpression
	:	INC
	;

postDecrementExpression
	:	postfixExpression DEC
	;

postDecrementExpression_lf_postfixExpression
	:	DEC
	;

castExpression
	:	LPAREN primitiveType RPAREN unaryExpression
	|	LPAREN referenceType additionalBound* RPAREN unaryExpressionNotPlusMinus
	|	LPAREN referenceType additionalBound* RPAREN lambdaExpression
	;

identifier : Identifier | TO | MODULE | OPEN | WITH | PROVIDES | USES | OPENS | REQUIRES | EXPORTS;

