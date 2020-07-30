# Importing ANTLR and Bison grammars

Antlrvsix can convert a few grammar types into
Antlr4 syntax.

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

### Remove unused options at the top of a grammar file.

There are many options no longer supported in Antlr4: _output_, _backtrack_,
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

### Remove options within optionSpec's at the beginning of rules.

Similar to the previous transform, _output_, _backtrack_,
_memoize_, _ASTLabelType_, _rewrite_
are removed from optionSpec at the beginning  of a rule.
If the optionsSpec is empty after removing these options,
the section itself is removed.

### Use new "tokens {}" syntax.

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

### Remove unsupported rewrite syntax and AST operators.

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

## Labels in lexer rules are not supported.

In Antlr3, one could write lexer rules that contained a label for an element.
This is no longer supported in Antlr4, so the label with "=" or "+=" are deleted.

    //rule_[id/TOKEN_REF]
        /altList
            //elementNoOptionSpec
                [EQUAL or PEQ]

### Lexer fragment rules cannot contain actions or commands.

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

### Fix options "k = ...".

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




