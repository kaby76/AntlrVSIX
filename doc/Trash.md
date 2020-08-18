# Trash, a shell for editing grammars

"Trash" is a command-line shell for parsing, converting, analyzing, and transforming
Antlr4 grammars. You can use the tool to automate refactorings
or to determine whether performance
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

<pre>
alias <em>id</em> = <u>string</u>
</pre>

Set up an alias that assigns <u>string</u> to <u>id</u>. The command <u>string</u> 
is executed with <u>id</u>.

### Analyze

_Trash_ can perform an analysis of a grammar. The analysis includes a count of symbol
type, cycles, and unused symbols.

<pre>
analyze
</pre>

### History expansion

_Trash_ keeps a persistent history of prior commands in ~/.trash.rc that is read
when the program starts. 
History expansions introduce words from the history list into the input stream,
making it easy to repeat commands. Currently there is no editing
capability.

<pre>
!!
</pre>

Execute the previous command.

<pre>
!<u>int</u>
</pre>

Execute the command line <u>int</u>.

<pre>
!<u>id</u>
</pre>

Execute the command that begins with <u>string</u>.

### Cd

<pre>
cd <u>string</u>?
</pre>

Change current directory. If string is not given, change to the user's home directory.

### Combine

<pre>
combine
</pre>

Combine two grammars on top of stack into one grammar.
One grammar must be a lexer grammar, the other a parser grammar,
order is irrelevant.

### Convert

<pre>
convert
</pre>

Convert the parsed grammar file at the top of stack into Antlr4 syntax. The
resulting Antlr4 grammar replaces the top of stack.

### "."

<pre>
.
</pre>

Print out the parse tree for the file at the top of stack.

### Delete

<pre>
delete <u>string</u>
</pre>

Delete nodes specified with the XPath expression string.

For for further details, see the [Delete parse tree node](refactoring.md#delete-parse-tree-node) refactoring details.

### Find

<pre>
find <u>string</u>
</pre>

Find all sub-trees in the parsed file at the top of stack using the given XPath expression string.

### Fold

<pre>
fold <u>string</u>
</pre>

Replace a sequence of symbols on the RHS of a rule
with the rule LHS symbol.

For for further details, see the [fold](refactoring.md#Fold) refactoring details.

### Foldlit

<pre>
fold <u>string</u>
</pre>

Replace a literal on the RHS of a rule
with the lexer rule LHS symbol.

For for further details, see the [fold literal](refactoring.md#replace-literals-in-parser-with-lexer-token-symbols) refactoring details.

### Has

<pre>
has (dr | ir) (left | right) <u>string</u>
</pre>

Print out whether the rule specified by the xpath expression pointing to the LHS symbol
of a parser or lexer rule has left or right recursion.

For for further details, see the [analysis](analysis.md#has-directindirect-recursion) section.

### History

<pre>
history
</pre>

Print out the shell command history.

### Kleene

<pre>
kleene <u>string</u>
</pre>

Replace a rule, whose symbol is identified by the xpath string,
of the grammar at the top of the grammar with an EBNF
form if it contains direct left or direct right recursion.

### Ls

<pre>
ls <u>string</u>
</pre>

List directory contents. If string is not given, list the current directory contents.

### Mvsr

<pre>
mvsr <u>string</u>
</pre>

Move the rule, whose symbol is identified by the xpath string,
to the top of the grammar.

### Parse

<pre>
parse (antlr2 | antlr3 | antlr4)?
</pre>

Parse the flie at the top of stack with the given parser type (_antlr2_, _antlr3, _antlr4_, or _bison_).

### Pop

<pre>
pop
</pre>

Pop the top document from the stack. If the stack is empty, nothing is further popped.
There is no check as to whether the document has been written to disk. If you want to write
the file, use `write`.

### Print

<pre>
print
</pre>

Print out text file at the top of stack.

### Quit

<pre>
(quit | exit)
</pre>

Exit the shell program.

### Read

<pre>
read <u>string</u>
</pre>

Read the text file _file-name_ and place it on the top of the stack.

### Rename

<pre>
rename <u>string</u> <u>string</u>
</pre>

Rename a symbol, the first parameter as specified by the xpath expression string,
to a new name, the second parameter as a string. The result may
place all changed grammars that use the symbol on the stack.

For for further details, see the [Rename](refactoring.md#rename) refactoring details.

### Reorder

<pre>
reorder alpha
reorder bfs <u>string</u>
reorder dfs <u>string</u>
</pre>

Reorder the parser rules according to the specified type. For
BFS and DFS, an XPath expression must be supplied to specify
all the start rule symbols. For alphabetic reordering, all parser
rules are retained, and simply reordered alphabetically. 
For BFS and DFS, if the rule is unreachable from a start node,
then the rule is dropped from the grammar.

### Rotate

<pre>
rotate
</pre>

Rotate the stack once.

### RR

<pre>
rr
</pre>

Replace left indirect or direct recursion with right recursion.

### Rup

<pre>
rup <u>string</u>?
</pre>

Find all altLists as specified by the xpath expression in the parsed file at the top of stack.
If the xpath expression is not given, the transform is applied to the whole file.
Rewrite the node with the parentheses removed, if the altList satifies three constraints:
(1) the expression must be a `altList` type in the Antlr4 grammar;
(2) the `altList` node doesn't contain more than one child, or if it does, then the containing altList/labeledAlt/alterative each does not contain more than one child;
(3) the `ebnf` parent of `block` must not contain a `blockSuffix`.

For for further details, see the [remove useless parentheses](refactoring.md#remove-useless-parentheses) refactoring details.

### Split

<pre>
split
</pre>

The `split` command attempts to split a grammar at the top of the stack.
The grammar must be a combined lexer/parser grammar for the transformation to
proceed. The transformation creates a lexer grammar and a parser grammar and places them
on the stack. The original grammar is popped off the stack.

For for further details, see the [split grammar](refactoring.md#splitting-and-combining-grammars) refactoring details.

### Stack

<pre>
stack
</pre>

Print the stack of files.

### ULLiteral

<pre>
ulliteral <u>string</u>?
</pre>

The ulliteral command applies the "upper- and lower-case string literal"
transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression.
If the xpath expression is not given, the transform is applied to the whole
file.
Prior to using this command,
the document must have been parsed.
The ulliteral operation substitutes a sequence of 
sets containing an upper and lower case characters
for a `STRING_LITERAL`.
The expression must point to the right-hand side `STRING_LITERAL` of
a parser or lexer rule.
The resulting code is parsed and placed
on the top of stack.

### Unalias

<pre>
unalias <u>id</u>
</pre>

Remove an aliased command.

### Unfold

<pre>
unfold <u>string</u>
</pre>

The unfold command applies the unfold transform to a collection of terminal nodes in the parse tree,
which is identified with the supplied xpath expression. Prior to using this command, you must have the file parsed.
An unfold operation substitutes the right-hand side of a parser or lexer rule
into a reference of the rule name that occurs at the specified node.
The resulting code is parsed and placed
on the top of stack.

### Unify

<pre>
unify <u>string</u>
</pre>

Perform a recursive left- and right- factorization of alternatives for rules.
The nodes specified must be for `ruleAltList`, `lexerAltList`, or `altList`.
A common prefix and suffix is performed on the alternatives, and
a new expression derived. The process repeats for alternatives nested. 

For for further details, see the [Unify alts to EBNF](refactoring.md#unify-alts-to-ebnf) refactoring details.

### Write

<pre>
write
</pre>

Pop the stack, and write out the file specified.
