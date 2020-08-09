# Trash -- a shell for performing transformations of Antlr grammars. 

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

### Analyze

### "!"

_Trash_ performs history expansion.
History expansions introduce words from the history list into the input stream,
making it easy to repeat commands, but only repeat previous commands. There is no editing
capability.

!! Execute the previous command.

!_n_ Execute the command line _n_.

!_string_ Execute the command that begins with _string_.

### Convert

### "."

### Find

### Fold

### History

### Parse

### Print

### Quit

### Read

### Rename

### Rotate

### Stack

### Unalias

### Unfold

### Write



