# Converting Antlr2, Antlr3, and Bison grammars into Antlr4

Antlrvsix can convert a few grammar types into
Antlr4 syntax. Either the Antlrvsix extension or the Trash
command-line tool can be used to convert grammars.

# Antlr3

The conversion of Antlr3 grammars into Antlr4 is
a relatively easy operation, but there are many steps
in the process. While there is no official document describing
how to convert an Antlr3 grammar to Antlr4, many of the steps
were outlined by Harwell in a [pull request he made to a grammar
file](https://github.com/senseidb/sensei/pull/23). While much of the
conversion may work, some remaining problmes will need to be manually
fixed.

## Transformations in Antlr3 to Antlr4 conversion

There are about a dozen or so transofmrations
that convert a grammar from Antlr3 syntax to
Antlr4. These are listed below. In each of the transformations,
an XPath expression is used to implement what nodes of the
parse tree to rewrite. Depending on the transformation, 
code is added to delete, construct new trees, and replace.

In reading the following XPaths that identify the relevant subtree,
please refer to the [Antlr3 lexer](https://github.com/kaby76/AntlrVSIX/blob/master/LanguageServer/ANTLRv3Lexer.g4)
and [parser](https://github.com/kaby76/AntlrVSIX/blob/master/LanguageServer/ANTLRv3Parser.g4) grammars.

### Remove unused options at the top of a grammar file

There are many grammar-level
options that were supported in Antlr3,
but are no longer supported in Antlr4: _output_, _backtrack_,
_memoize_, _ASTLabelType_, _rewrite_. Import removes these options.
If the optionsSpec is empty after removing these options,
the section itself is removed.

    //grammarDef/optionsSpec
        /option
            [id
                /(TOKEN_REF | RULE_REF)
                [text() = 'output'
                or text() = 'backtrack'
                or text() = 'memoize'
                or text() = 'ASTLabelType'
                or text() = 'rewrite'
                ]]

### Remove options within optionSpec's at the beginning of rules

Similar to the previous transform, _output_, _backtrack_,
_memoize_, _ASTLabelType_, _rewrite_
are removed from optionSpec at the beginning  of a rule.
If the optionsSpec is empty after removing these options,
the section itself is removed.

### Use new "tokens {}" syntax

In Antlr4, the _tokens { ... }_ syntax was changed from semi-colon delimited identifiers
to comma delimited identifiers. The last item in the _tokens_ list cannot have a trailing
comma, so it is removed.

    //tokensSpec
        /tokenSpec
            /SEMI

In addition, the assignment of a string literal
value to a token in the tokens list is no longer supported, so "FOO = 'foo';"
is changed to "FOO" in the tokens list,
 and a lexer rule added in its place at
 the end of the new grammar
file, e.g., "FOO: 'foo' ;".

    //tokensSpec
        /tokenSpec[EQUAL]

    ( rule_ {lhs} {colon}
        ( altList "
            ( alternative
                ( element
                    ( elementNoOptionSpec
                        ( atom
                            {rhs}
        )   )   )   )   )
        {semi}
    )

### Remove unsupported rewrite syntax and AST operators

Antlr4 makes a strong departure from Antlr3 in purpse: AST construction is
not a supported feature anymore because Antlr is not for compiler construction.
Consequently, a number of anotations are nuked from the Antlr3 parse tree,
including

* "!" and "^" operators.

        //atom
             /(ROOT | BANG)

        //terminal_
             /(ROOT | BANG)

        //rule_/BANG

* Rewrite nodes, which include the -> operator

        //rewrite

### Scopes

Antlr4 supports _scope_ with _local_. However, this tool
currently simply deletes the _scope_ clause.

    //rule_/ruleScopeSpec

### Labels in lexer rules are not supported

In Antlr3, one could write lexer rules that contained a label for an element.
This is no longer supported in Antlr4, so the label with "=" or "+=" are deleted.

    //rule_[id/TOKEN_REF]
        /altList
            //elementNoOptionSpec
                [EQUAL or PEQ]

### Lexer fragment rules cannot contain actions or commands

Again, while actions were allowed in _fragment_ rules in the lexer, these
are no longer supported so actions are deleted.

    //rule_[FRAGMENT]
        /altList
            //elementNoOptionSpec
                /actionBlock[not(QM)]

### Syntactic predicates are no longer supported in Antlr4

Syntactic predicates were supported in Antlr3 to handle parsing problems.
These are no longer needed, and so they are deleted.

    //ebnf [SEMPREDOP]

### Semantic predicates do not need to be explicitly gated in ANTLR 4

The gating of sematic predicates is unnecessary in Antlr4, so are removed.

    //elementNoOptionSpec
        [(actionBlock and QM)]
            /SEMPREDOP

### Fix options "k = ..."

The "k" option is not needed in Antlr4 since it is LL(*). The "greedy"
option is replaced with the "*?* operator.

    //optionsSpec[not(../@id = 'grammarDef')]
        /option
            [id
                /(TOKEN_REF | RULE_REF)
                [text() = 'k'
                ]]

    //optionsSpec[not(../@id = 'grammarDef')]
        /option
            [id/(TOKEN_REF | RULE_REF)[text() = 'greedy']
                and 
             optionValue/id/RULE_REF[text() = 'false']]
             
## Problems remaining that require manual fixes

After conversion, you will need to check and correct any remaining problems.

* The tool does not currently convert embedded code in action blocks (it may
at some point).
* Gated semantic predicates in Antlr3 are not exactly equivalent to the semantic
predicates in Antlr4. If you use `@init { ... }` to declare variables for the rule,
they won't be visible in the scope of the declared predicate. See 
[this](https://github.com/kaby76/AntlrVSIX/blob/master/UnitTestProject1/Numbers.g3) example.


# Antlr2

Antlr2 grammars are a little more challenging to convert to Antlr4, but there
is a pretty good guide to
[converting Antlr2 to Antlr3 by Parr](https://theantlrguy.atlassian.net/wiki/spaces/ANTLR3/pages/2687070/Migrating+from+ANTLR+2+to+ANTLR+3).

In reading the following XPaths that identify the relevant subtree,
please refer to the [Antlr2 lexer](https://github.com/kaby76/AntlrVSIX/blob/master/LanguageServer/ANTLRv2Lexer.g4)
and [parser](https://github.com/kaby76/AntlrVSIX/blob/master/LanguageServer/ANTLRv2Parser.g4) grammars.

### Remove header

Antlr2 allows one to define a `header_` at the top of a grammar file. Unfortunately,
this isn't supported in Antlr4. The header is deleted.

    //header_

### Remove classDef action blocks (for now)

In each `classDef`, an `actionBlock` can occur as the first child node. For now,
this sub-tree is removed, but may be converted in the future.

    //classDef/actionBlock

### Removed unsupported options

The `option` sub-tree can appear in a number of places in the grammar.
Many options that were available in Antlr2 are now unnessary or unsupported
in Antlr4. These options are removed and the entire `optionsSpec` removed
if there it does not enclose any options.

    //(fileOptionsSpec | parserOptionsSpec | lexerOptionsSpec | treeParserOptionsSpec)
        /(option | lexerOption)
            [id/*
                [text() = 'output'
                or text() = 'backtrack'
                or text() = 'memoize'
                or text() = 'ASTLabelType'
                or text() = 'rewrite'
                or text() = 'k'
                or text() = 'exportVocab'
                or text() = 'testLiterals'
                or text() = 'interactive'
                or text() = 'charVocabulary'
                or text() = 'defaultErrorHandler'
                ]]

### Parser and Lexer in One Definition

Parr supported parser and lexer grammars in a single file using
"class" declarations. In version 3 of Antlr, Parr took this further
by just dropping the partioning altogether, allowing lexer and parser
rules to be intermingled. So, in convert to version 4, I simply remove
the declaration except for the first one, and turn it into a combined
grammar. If there's only one class declaration, I won't remove it, but
will change it into the current syntax.


# Bison

