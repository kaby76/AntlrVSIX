# Building and Running Antlr Applications

Support for Antlr grammars in C# projects is provided
by [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks).
Antlrvsix focuses on the editing of Antlr grammars.

Antlr4BuildTasks is a standard package which you can [reference using NuGet](https://www.nuget.org/packages/Antlr4BuildTasks/).
Also including in the Antlr4BuildTasks git tree is Antlr4BuildTasks.Templates. This Nuget
package includes Net Core templates for generating a C# Antlr program.

## Prerequisites

* You must install the Net Core toolkit.
* You will need an internet connection in order for 
dependencies to be downloaded. 

## Adding Build Rules to an Existing Project

To add building capability to your Antlr program,
add a reference to Antlr4BuildTasks to the
library or application which contains the .g4 grammar files.
_NB: Do not include the generated .cs Antlr parser files
in the CSPROJ file for your program._ Instead, the generated
parser code is placed in the build temp output directory along with
the .obj files from the C# compiler.

To build and run a program,
restore packages for the solution, then "F5".

## Multi-file grammars

AntlrVSIX and Antlr4BuildTasks handles multi-file grammars, e.g., a SomethingLexer.g4 and SomethingParser.g4. Simply add them
to your project, and make sure the Build Action under the File Properties dialog box is set
to "Antlr4".

If a grammar file imports another grammar file (via an import statement, e.g.,
"import LexBasic; // Standard set of fragments"),
make sure to set the Build Action under the File Properties dialog box to "None", not
"Antlr4". Otherwise, Antlr will be called for this imported file; Antlr should only be invoked
on the top-level grammar file.

## Creating an Antlr Application from the Project Templates

To create an Antlr application, in a command-line prompt,
type "dotnet new antlr". You can use Visual Studio to build, run, and debug the code.

The template program is a complete example of Antlr:
* grammar is for complex expressions;
* creates a parser and lexer, and runs it on a simple expression;
* outputs a parenthesized-representation of the parse tree;
* creates a tree visitor that computes the synthesized value of each subexpression,
and runs the visitor on the parse tree.

From this template, you should be able to tackle more complex grammars, including
C++ (with preprocessor), C#, Java, Python, etc.

<br/><img src="pics/2019-08-08-34.png" width="75%" />

<br/><img src="pics/2019-08-08-33.png" width="75%" />

[Next: Customizing the keyboard](customizing.md)<br/>
