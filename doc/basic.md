# Basic Editing of a Grammar

__Prior to using the AntlrVSIX extension, you should install the Java runtime, the "complete" Antlr tool
jar, and update your environment with JAVA_HOME, Antlr4ToolPath. You should have this so you can perform
builds. You can use the extension without these, but you won't be able to build and debug your Antlr program.__

AntlrVSIX supports editing of Antlr version 4 grammars. Grammar files
must end with a .g or .g4 suffix (the "standard" is to use .g4).
A grammar that only partially parses will show tagging up to the error.
Grammars can be split into multiple files, but they should be in the same directory.
The tool will try to only perform cross symbol matching between lexer and parser grammars
by the tokenVocab option, or the import statement.

AntlrVSIX is implemented using a Language Server Protocol API provided by Microsoft.

## Tagging

AntlrVSIX support tagging of terminal and non-terminal grammar symbols. Literals are not currently tagged.
Tagging only works from the beginning of the file to a point of syntax error.

## Intellisense Tooltip

AntlrVSIX provides tool tips of the type of the symbol when you move the mouse over
it. You can, of course, gather the type of symbol by Antlr rules (all terminals begin with an uppercase
letter, non-terminals begin with a lowercase letter).

## Intellisense Command Completion

AntlrVSIX provides some command completion suggestions. When the user types, the tool provides
a list of non-terminals and terminals currently in use in the grammar file. Note, it does not
yet suggest symbols of imported grammars unless they are already in use.

## Go to Definition

An easy but important way to navigate around the grammar is "go to definition" of a 
terminal or non-terminal. When right-clicking on a symbol, a context menu is opened
containing the "Go To Definition", which you can select. The editor
will open the grammar and place the caret at the beginning of the rule for the symbol. AntlrVSIX
looks at all grammar files, selecting first the grammar file that is within the same directory
as the applied occurrence of the symbol. If AntlrVSIX does not find a defining rule for the symbol,
"Go To Definition" will not move the cursor.
You can use the "back arrow" button in the toolbar of VS to go back to the applied occurrence.

Right-click on symbol, select "Go To Definition": <br/><img src="pics/2020-03-08 (1).png" width="75%" />

Result of "Go To Definition": <br/><img src="pics/2020-03-08 (2).png" width="75%" />


## Find All References

To find all references and definition of a grammar symbol, right-click on a symbol
then select the "Find All References". Visual Studio will open a window containing the results.
Select an occurrence to navigate to the result.

Right-click on symbol, select "Find All References": <br/><img src="pics/2020-03-08 (3).png" width="75%" />

Visual Studio will display the find references window: <br/><img src="pics/2020-03-08 (4).png" width="75%" />

Select a line to navigate to the occurrence: <br/><img src="pics/2020-03-08 (5).png" width="75%" />


## Go to Visitor/Listener

AntlrVSIX provides a way to navigate from a non-terminal symbol in the grammar to a visitor
or listener method for an IParseTree node of a parse tree corresponding to the symbol.
First, select the symbol you are interested in. Then, in the main menu select "Extensions -> AntlrVSIX
-> Navigate -> Go To Listener" or "AntlrVSIX
-> Navigate -> Go To Visitor". Note, by default, AntlrVSIX will not generate any method if it does not
find the method. Otherwise, you can change the settings for AntlrVSIX to do that in the Options for
the extension.

Note, there are two "listeners" per each symbol: Enter#symbol# and Exit#symbol#. By default,
AntlrVSIX navigates to the Enter#Symbol# method. If you want to navigate to the Exit#symbol# method,
press the Control-key while selecting "Go to listener". Note, for visitors, there is only one method.

Right-click on symbol, select "Go to listener": <br/><img src="pics/2019-08-08-26.png" width="75%" />

Result of navigation: <br/><img src="pics/2019-08-08-30.png" width="75%" />

[Next: Building and Running Antlr Applications](building.md)<br/>
