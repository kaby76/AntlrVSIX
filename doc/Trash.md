# Trash -- a shell for performing transformations of Antlr grammars

"Trash" is a simple command-line shell for parsing, converting, analyzing, and transforming
Antlr grammars. You can use the tool to automate changes to a grammar,
or use it to determine whether performance
problems that you may experience in VS are due to
VS or the Antlr language server.

## Commands

### Alias

Aliases allow a string to be substituted for a word when it is used as the
first word of a simple command. The shell maintains a list of aliases that
may be set and unset with the alias and unalias builtin commands.

The first word of each simple command is checked to see if it has an alias.
If so, that word is replaced by the text of the alias.
Only [a-zA-Z_] appear in an alias name. The replacement text may contain
any valid shell input, including shell metacharacters. The first word of
 the replacement text is tested for aliases, but a word that is identical
 to an alias being expanded is not expanded a second time. This means that
 one may alias ls to "ls -F", for instance, and Bash does not try to recursively
 expand the replacement text. If the last character of the alias value is a
 blank, then the next command word following the alias is also checked
 for alias expansion.

Aliases are created and listed with the alias command, and removed with the
 unalias command.

`alias id=string`

* Set up an alias that assigns _string_ to _id_. The command _string_ 
is executed with _id_.

### Analyze

_Trash_ can perform an analysis of a grammar. The analysis includes a count of symbol
type, cycles, and unused symbols.

`analyze`


### History expansion

_Trash_ keeps a persistent history of prior commands in ~/.trash.rc that is read
when the program starts. 
History expansions introduce words from the history list into the input stream,
making it easy to repeat commands. Currently there is no editing
capability.

`!!`

* Execute the previous command.

`!n`

* Execute the command line _n_.

`!string`

* Execute the command that begins with _string_.

### Convert

`convert`

* Convert the parsed grammar file at the top of stack into Antlr4 syntax. The
resulting Antlr4 grammar replaces the top of stack.

### "."

`.`

* Print out the parse tree for the file at the top of stack.

### Delete

`delete xpath-expression`

* Delete nodes specified with the XPath expression.

For for further details, see the [Delete parse tree node](refactoring.md#delete-parse-tree-node) refactoring details.

### Find

`find xpath-string`

* Find all sub-trees in the parsed file at the top of stack using the given XPath expression.

### Fold

### History

`history`

* Print out the shell command history.

### Mvsr

`mvsr xpath-string`

* Move the rule, whose symbol is identified by the xpath string,
to the top of the grammar.

### Parse

`parse grammar-type`

* Parse the flie at the top of stack with the given parser type (_antlr2_, _antlr3, _antlr4_, or _bison_).

### Pop

`pop`

* Pop the top document from the stack. If the stack is empty, nothing is further popped.
There is no check as to whether the document has been written to disk. If you want to write
the file, use `write`.

### Print

`print`

* Print out text file at the top of stack.

### Quit

`quit` or `exit`

* Exit the shell program.

### Read

`read file-name`

* Read the text file _file-name_ and place it on the top of the stack.

### Rename

`rename xpath-expression new-name`

* Rename a symbol in the current grammar. The result may pop
the stack and place all related grammars that use or define the symbol
on the stack.

For for further details, see the [Rename](refactoring.md#rename) refactoring details.

### Reorder

`reorder alpha`
`reorder bfs xpath-expression`
`reorder dfs xpath-expression`

* Reorder the parser rules according to the specified type. For
BFS and DFS, an XPath expression must be supplied to specify
all the start rule symbols. For alphabetic reordering, all parser
rules are retained, and simply reordered alphabetically. 
For BFS and DFS, if the rule is unreachable from a start node,
then the rule is dropped from the grammar.

### Rotate

`rotate`

* Rotate the stack once.

### Rup

`rup xpath-expression`

* Find all blocks as specified by the xpath expression in the parsed file at the top of stack.
Rewrite the node with the parentheses removed, if the block satifies three constraints:
(1) the expression must be a `block` type in the Antlr4 grammar;
(2) the `block` node must have an `altList` that does not contain more than one child;
(3) the `ebnf` parent of `block` must not contain a `blockSuffix`.

For for further details, see the [remove useless parentheses](refactoring.md#remove-useless-parentheses) refactoring details.

### Split

`split`

* The `split` command attempts to split a grammar at the top of the stack.
The grammar must be a combined lexer/parser grammar for the transformation to
proceed. The transformation creates a lexer grammar and a parser grammar and places them
on the stack. The original grammar is popped off the stack.

For for further details, see the [split grammar](refactoring.md#splitting-and-combining-grammars) refactoring details.

### Stack

`stack`

* Print the stack of files.

### ULLiteral

`ulliteral xpath-expr`

* The ulliteral command applies the "upper- and lower-case string literal"
transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression. Prior to using this command,
the document must have been parsed.
The ulliteral operation substitutes a sequence of 
sets containing an upper and lower case characters
for a `STRING_LITERAL`.
The expression must point to the right-hand side `STRING_LITERAL` of
a parser or lexer rule.
The resulting code is parsed and placed
on the top of stack.

### Unalias

`unalias id`

* Remove an aliased command.

### Unfold

`unfold xpath-expr`

* The unfold command applies the unfold transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression. Prior to using this command, you must have the file parsed.
An unfold operation substitutes the right-hand side of a parser or lexer rule
into a reference of the rule name that occurs at the specified node.
The resulting code is parsed and placed
on the top of stack.

### Unify

`unify xpath-expression`

* Perform a recursive left- and right- factorization of alternatives for rules.
The nodes specified must be for `ruleAltList`, `lexerAltList`, or `altList`.
A common prefix and suffix is performed on the alternatives, and
a new expression derived. The process repeats for alternatives nested. 

For for further details, see the [Unify alts to EBNF](refactoring.md#unify-alts-to-ebnf) refactoring details.

### Write

`write`

* Pop the stack, and write out the file specified.




