# AntlrVSIX

AntlrVSIX is a tool to support editing, analysis, refactoring,
and conversion of Antlr2, Antlr3, Antlr4, Bison and W3C EBNF grammars. It contains
extensions for Visual Studio 2019, Visual Studio Code, Gnu Emacs,
a [Language Server Protocol (LSP)](https://langserver.org/) server,
and a stand-alone command-line tool
known as [Trash](https://github.com/kaby76/AntlrVSIX/blob/master/doc/Trash.md)
for editing grammar files directly without
Visual Studio, runnable on Windows or Linux.
All clients use the LSP server implemented in this repository. The
clients each are implemented elsewhere, but a thin shim is provided in this
repository to get the client to work with the server.
For Visual Studio 2019, the client is implemented using Microsoft's 
[client](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Client/).
The server is implemented using
[Antlr](https://www.antlr.org/), [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks),
[XPath2](https://en.wikipedia.org/wiki/XPath), 
[S-expressions](https://en.wikipedia.org/wiki/S-expression)
for Antlr parse trees,
and a number of other tools.
Most of the code is implemented in C#.
The extension for VS Code is written in Typescript.
Supported are colorized tagging, hover, go to def, find all refs,
replace, and reformat. Beyond the basic LSP commands, the tool supports
analysis, refactoring, and conversion.

## Supported grammars and features

| Grammars | File suffix | Features supported |
| ---- | ---- | ---- |
| Antlr4 | .g4 | Basic LSP services (tagging, go to def, find all refs, rename, etc.); transformations; analysis |
| Antlr3 | .g3 | Basic LSP services (tagging, go to def, find all refs, rename, etc.); conversion to Antlr4 |
| Antlr2 | .g2 | Basic LSP services (tagging, go to def, find all refs, rename, etc.); conversion to Antlr4 |
| Bison | .y | Basic LSP services (tagging, go to def, find all refs, rename, etc.); conversion to Antlr4 |
| W3C EBNF | .ebnf | Basic LSP services (tagging, go to def, find all refs, rename, etc.); conversion to Antlr4 |

# Installation

## Visual Studio 2019

For Visual Studio 2019, I recommend that you use the latest, upgraded version since it has the latest bug fixes.
To install, visit the [Visual Studio Marketplace for VS2019 for Antlrvsix](https://marketplace.visualstudio.com/items?itemName=KenDomino.AntlrVSIX). Download and install the .vsix file. You can also install it through the `Manage Extensions`
menu item in Visual Studio 2019.

If you find this extension not to your liking, you can try to use the extension for Visual Studio Code.
I found that it is sometimes easier to install [Open in Visual Studio Code](https://github.com/madskristensen/OpenInVsCode/)
and edit the grammar in VSCode due to VS2019 not directly supporting LSP semantic highlighting.

## Visual Studio Code

For Visual Studio Code, visit the [Visual Studio Marketplace for VSCode for Antlrvsix](https://marketplace.visualstudio.com/items?itemName=KenDomino.Antlrvsix-vscode). Download and install the the .vsix file. NB: the vsix file between VS2019 and VSCode are
not the same files.

## GNU Emacs

For Emacs, you will need to follow the instructions [here](https://github.com/kaby76/AntlrVSIX/tree/master/Emacs).

## Alternatives

Mike Lischke's
[vscode-antlr4](https://github.com/mike-lischke/vscode-antlr4) is a good tool for VSCode for Antlr grammars, which
you may prefer. However, Antlrvsix contains
a command-line tool that can be used to automate transformations on an
Antlr grammar, or want to quickly run a parser over a grammar file and
use XPath to query items in the parse tree. There is no tool like it, and it is quite powerful.

If you are just building and running an Antlr application and don't care about
editing features (i.e., you don't care for highlighting the grammar, go to def, etc.),
then you just use [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks). Antlr4BuildTasks
is a replacement of [Antlr4cs](https://github.com/tunnelvisionlabs/antlr4cs) and the
build tool contained in that library. The Antlr4cs is several versions behind the latest Antlr release.

## Nightly build

Each night at 0h 0m UTC, Appveyor builds the latest source. The output of the build can be downloaded
at https://ci.appveyor.com/api/projects/kaby76/antlrvsix/artifacts/VsIde/bin/Debug/AntlrVSIX.vsix.
However, there is no guarantee that this version will work.

# Documentation

## Guide to basic features

For information on how basic use, see the [User Guide](https://github.com/kaby76/AntlrVSIX/blob/master/doc/readme.md).

## Running the **Trash** command-line engine

The **Trash** tool is a command-line interpreter for executing
operations on grammar files. With it, you can read Antlr2, Antlr3,
Antrl4, and Bison grammars, convert to Antlr4, find sub-trees
in the parse tree for the grammar using XPath expressions, analyze,
and refactor the grammar.

To run the tool, you will need to find the **Trash.exe** executable
after installing the extension in Visual Studio 2019, or unpacking
it directly from the downloaded .vsix--a ZIP file.
It should be under
"C:/Users/userid/AppData/Local/Microsoft/VisualStudio/.../AntlrVSIX/8.0/Trash/netcoreapp3.1/Trash.exe".
Once you find the executable, please refer to
the [documentation](https://github.com/kaby76/AntlrVSIX/blob/master/doc/Trash.md).

## Analysis

### Recursion

* [Has direct/indirect recursion](https://github.com/kaby76/AntlrVSIX/blob/master/doc/analysis.md#has-directindirect-recursion)

## Refactoring

Antlrvsix provides a number of transformations that can help to make grammars cleaner (reformatting),
more readable (reducing the length of the RHS of a rule),
and more efficient (reducing the number of non-terminals) for Antlr.

Some of these refactorings are very specific for Antlr due to the way
the parser works, e.g., converting a prioritized chain of productions recognizing
an arithmetic expression to a recursive alternate form.
The refactorings implemented are:

### Raw tree editing

* [Delete parse tree node](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#delete-parse-tree-node)

### Reordering

* [Move start rule to top](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#move-start-rule)
* [Reorder parser rules](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#reorder-parser-rules)
* [Sort modes](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#sort-modes)

### Changing rules

* [Remove useless parentheses](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#remove-useless-parentheses)
* [Remove useless parser rules](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#remove-useless-productions)
* [Rename lexer or parser symbol](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#rename)
* [Unfold](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#Unfold)
* [Group alts](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#group-alts)
* [Ungroup alts](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#ungroup-alts)
* [Upper and lower case string literals](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#upper-and-lower-case-string-literals)
* [Fold](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#Fold)
* Replace direct left recursion with right recursion
* [Replace direct left/right recursion with Kleene operator](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#Kleene)
* Replace indirect left recursion with right recursion
* Replace parser rule symbols that conflict with Antlr keywords
* [Replace string literals in parser with lexer symbols](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#replace-literals-in-parser-with-lexer-token-symbols)
* Replace string literals in parser with lexer symbols, with lexer rule create
* [Delabel removes the annoying and mostly useless labeling in an Antlr grammar](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#delabel)

### Splitting and combining

* [Split combined grammars](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#splitting-and-combining-grammars)
* [Combine splitted grammars](https://github.com/kaby76/AntlrVSIX/blob/master/doc/refactoring.md#splitting-and-combining-grammars)

## Conversion

* [Antlr3 import](https://github.com/kaby76/AntlrVSIX/blob/master/doc/Import.md#antlr3)
* [Antlr2 import](https://github.com/kaby76/AntlrVSIX/blob/master/doc/Import.md#antlr2)
* [Bison import](https://github.com/kaby76/AntlrVSIX/blob/master/doc/Import.md#bison)

---------

The source code for the extension is open source, free of charge, and free of ads. For the latest developments on the extension,
check out my [blog](http://codinggorilla.com).

# Building Antlrvsix

[![Build status](https://ci.appveyor.com/api/projects/status/ekyhff3c28p5qgox?svg=true)](https://ci.appveyor.com/project/kaby76/antlrvsix)

* From a clean "git" root directory, open a "Developer command prompt", and execute:
  * msbuild /t:restore
  * msbuild

The extension is at ./Client/bin/Debug/AntlrVSIX.vsix after building successfully.

* If you want to make modifications for yourself, you should [reset your
Experimental Hive for Visual Studio](https://docs.microsoft.com/en-us/visualstudio/extensibility/the-experimental-instance?view=vs-2017). To do that,
Microsoft recommends using CreateExpInstance.exe.
Unfortunately, I've found CreateExpInstance doesn't always work because it copies from
previous hives stored under the AppData directory. It is often easier to
just recursively delete all directories ...\AppData\Local\Microsoft\VisualStudio\16.0_*. I've included a
Bash script "clean.sh" in the Antlrvsix source to clean out the build files so one can build from scratch.


---------

# Prior Releases

See [this guide](https://github.com/kaby76/AntlrVSIX/blob/master/PriorReleases.md).

# Roadmap

## Current release v8.1 (21 Oct 2020):

This release is a mix of organizational and feature changes.
There are some important bug fixes associated with synchronization (#87, #88, #89),
and performance (#82, #90, #96). Getting a little bored, and realizing that it is
time to take advantage of the LSP server, I added two new clients to support
now three clients: VS2019, Emacs, and VSCode. (I am working on
the IntellijIdea and VIM clients.)
And, I added a few new commands to Trash.
Under the covers, I replaced a basic library that I was using
from Microsoft for the [Microsoft.VisualStudio.LanguageServer.Protocol](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/),
with one I wrote, which ended taking a week of 12+ hour days.
Not being able myself to remember the commands of Trash--because it is now
getting pretty large--I decided to add help, and reorganize the
underlying command-line interpreter.
A number of things I wanted to get done for transforming the Java grammar
from the spec I had to push off to the next release in order to deal
with more critical issues.

There are still some significant problems with semantic highlighting and synchronization,
but I will work through these in due time.

* Fix ["rup" doesn't work in some cases #81](https://github.com/kaby76/AntlrVSIX/issues/81)
* Fix ["has dr" not working, slow as hell #82](https://github.com/kaby76/AntlrVSIX/issues/82)
* Fix [Rename "unify" to "group" #86](https://github.com/kaby76/AntlrVSIX/issues/86)
* Fix [When typing fast, the LSP packets can be delivered to the server out of order. #87](https://github.com/kaby76/AntlrVSIX/issues/87)
* Fix [Killing the server does not go back to a good state of the text document. #88](https://github.com/kaby76/AntlrVSIX/issues/88)
* Fix [JSON transport api is erroneously multi-threaded #89](https://github.com/kaby76/AntlrVSIX/issues/89)
* Fix [Performance still an issue #90](https://github.com/kaby76/AntlrVSIX/issues/90)
* Fix [ANTLR3 grammars are wrong #93](https://github.com/kaby76/AntlrVSIX/issues/93)
* Fix [Selecting type of file should be respected by the DidOpen call. #94](https://github.com/kaby76/AntlrVSIX/issues/94)
* Fix [Error recovery in server not considering lexer errors #95](https://github.com/kaby76/AntlrVSIX/issues/95)
* Fix [Performance Problems #96](https://github.com/kaby76/AntlrVSIX/issues/96)
* Fix [Links to User Guide and Documentation are broken #97](https://github.com/kaby76/AntlrVSIX/issues/97)
* Add Visual Studio Code client to Marketplace.
* Add Gnu Emacs extension.
* Add WC3 EBNF parsing and conversion.
* Add comment lines in Trash.
* Add Help command in Trash.
* Add Ungroup transform in Trash.
* Add Delabel transform in Trash.
* Replace [Microsoft.VisualStudio.LanguageServer.Protocol](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/) with a drop-in replacement in order to handle semantic highlighting.

## Planned for v8.2 (beginning of Nov 2020)

* Add Intellij plugin.
* Add existing transforms not in Trash to Trash. Make sure they work.
* Add expression rule optimization. Verify that this works with Java.
* Add left factoring and inverse? Not sure, as Unify is a superset of left factoring.
* Add in empty string alternative hoist transforms.
* Add grammar diff, think about 3-way grammar merge.
* Add ISO 14977 EBNF.

## Planned for v9 (end Dec 2020)

* Rebrand.
* Add flexible parser to read any grammar.
* Add AST for grammars. Rewrite all with ASTs for syntax-independent code.


Any questions, email me at ken.domino <at> gmail.com
