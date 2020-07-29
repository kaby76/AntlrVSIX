# Importing ANTLR and Bison grammars

Antlrvsix can convert a few grammar types into
Antlr4 syntax.

# Antlr3

The conversion of Antlr3 grammars into Antlr4 is
a relatively easy operation, but there are many steps
in the process. While there is no official document describing
how to convert an Antlr3 grammar to Antlr4, many of the steps
were outlined by Harwell in a [pull request he made to a grammar
file](https://github.com/senseidb/sensei/pull/23).

## Renove unused options at the top of a grammar file.

There are many options no longer supported in Antlr4: _output_, _backtrack_,
_memoize_, _ASTLabelType_, _rewrite_. These options are removed from the
optionsSpec section. If the optionsSpec is empty after removing these options,
the section is removed.

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

## Remove options within optionSpec's at the beginning of rules.

Similar to the previous transform, optionSpec's at the beginning of rules require
clean up.

## Use new "tokens {}" syntax.

In Antlr4, the _tokens { ... }_ syntax was changed from semi-colon delimited values
to comma delimited values. The last item in the _tokens_ list does not have a trailing
comma, so it is removed.

    //tokensSpec
        /tokenSpec
            /SEMI

In addition, the assignment of token strings is no longer supported, so "FOO = 'foo';"
is changed to "FOO", and a lexer rule added in its place at the end of the new grammar
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

## Remove unsupported rewrite syntax and AST operators.

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

## Scopes

Antlr4 supports _scope_ with _local_. However, this tool
currently simply deletes the _scope_ clause.

    //rule_/ruleScopeSpec
