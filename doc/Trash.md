# Trash -- a shell for performing transformations of Antlr grammars

"Trash" is a simple command-line shell for reading, converting, transforming,
and writing Antlr grammars. You can use this to convert Bison, Antlr2, and Antlr3
grammars to Antlr4, analyze or make changes to the grammar. You don't need to run
Visual Studio to use the tool, and you can use it to determine whether performance
problems you see while using the extension in VS is due to VS or with the
language server for Antlr.

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

_Trash_ keeps a persistent history of prior commands for repeated execution.
History expansions introduce words from the history list into the input stream,
making it easy to repeat commands, but only repeat previous commands. There is no editing
capability.

`!!`

* Execute the previous command.

`!n`

* Execute the command line _n_.

`!string`

* Execute the command that begins with _string_.

### Convert

`convert`

* Convert the parsed grammar file at the top of stack into Antlr4 syntax, and leave it at the top of stack.

### "."

`.`

* Print out the parse tree for the file at the top of stack.

### Find

`find xpath-string`

* Find all sub-trees in the parsed file at the top of stack using the given XPath expression.

### Fold

### History

`history`

* Print out the shell command history.

### Parse

`parse grammar-type`

* Parse the flie at the top of stack with the given parser type (_antlr2_, _antlr3, _antlr4_, or _bison_).

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

### Rotate

### Stack

`stack`

* Print the stack of files.

### Unalias

### Unfold

### Write

`write`

* Pop the stack, and write out the file specified.




