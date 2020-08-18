# AntlrVSIX

AntlrVSIX is a tool to support editing, analysis, refactoring,
and conversion of Antlr2, Antlr3, Antlr4, and Bison grammars. It contains
an extension for Visual Studio 2019 and a stand-alone command-line tool
known as [Trash](doc/Trash.md) for editing grammar files directly without
Visual Studio, runnable on Windows or Linux.
It is implemented using Microsoft's [Language Server Protocol (LSP)](https://langserver.org/) 
[client](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Client/) and
[server](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/) APIs,
[Antlr](https://www.antlr.org/), [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks),
[XPath2](https://en.wikipedia.org/wiki/XPath), 
[S-expressions](https://en.wikipedia.org/wiki/S-expression)
for Antlr parse trees,
and a number of other tools.
Most of the extension is implemented in C#.
There is a client for VS Code, written in Typescript,
but I am deferring further development of the client until the server is more or less complete.
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

# Installation

Antlrvsix supports Visual Studio 2019.
I recommend that you use version 16.7 since it has the latest bug fixes.
If you find this extension not to your liking, use VSCode and Mike Lischke's
[vscode-antlr4](https://github.com/mike-lischke/vscode-antlr4). Antlrvsix contains
a command-line tool if you want to automate transformations on a
Antlr grammar, or want to quickly run a parser over a grammar file and
use XPath to query items in the parse tree.

If you are just building and running an Antlr application and don't care about
editing features, then you just need Net Core 3.1. Builds are supported 
using [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks).

You can install the extension in one of four ways:

* [Download the .vsix from the Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=KenDomino.AntlrVSIX)
with a web browser, then executing the .vsix file from a Windows Explorer.
* Download and install the extension within Visual Studio 2019 via "Extensions | Manage Extensions",
search for Antlrvsix, and "install".
* Build a copy of the .vsix from the sources in this repository and install. NB: I sometimes check in code
into the repository that does not compile. This is because I alone make changes to the code, and it is undergoing
huge changes.
Grab the source for a released version to be safe.
* Each night at 0h 0m UTC, Appveyor builds the latest source. The output of the build can be downloaded
at https://ci.appveyor.com/api/projects/kaby76/antlrvsix/artifacts/Client/bin/Debug/AntlrVSIX.vsix.
However, there is no guarantee that this version will work.

# Documentation

## Guide to basic features

For information on how basic use, see the [User Guide](doc/readme.md).

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
Once you find the executable, execute it.

For a list of commands in Trash, see this [documentation](doc/Trash.md).

## Analysis

### Recursion

* [Has direct/indirect recursion](doc/analysis.md#has)

## Refactoring

Antlrvsix provides a number of transformations that can help to make grammars cleaner (reformatting),
more readable (reducing the length of the RHS of a rule),
and more efficient (reducing the number of non-terminals) for Antlr.

Some of these refactorings are very specific for Antlr due to the way
the parser works, e.g., converting a prioritized chain of productions recognizing
an arithmetic expression to a recursive alternate form.
The refactorings implemented are:

### Raw tree editing

* [Delete parse tree node](doc/refactoring.md#delete-parse-tree-node)

### Reordering

* [Move start rule to top](doc/refactoring.md#move-start-rule)
* [Reorder parser rules](doc/refactoring.md#reorder-parser-rules)
* [Sort modes](doc/refactoring.md#sort-modes)

### Changing rules

* [Remove useless parentheses](doc/refactoring.md#remove-useless-parentheses)
* [Remove useless parser rules](doc/refactoring.md#remove-useless-productions)
* [Rename lexer or parser symbol](doc/refactoring.md#rename)
* [Unfold](doc/refactoring.md#Unfold)
* [Unify alts to EBNF](doc/refactoring.md#unify-alts-to-ebnf)
* [Upper and lower case string literals](doc/refactoring.md#upper-and-lower-case-string-literals)
* [Fold](doc/refactoring.md#Fold)
* [Replace direct left recursion with right recursion]()
* [Replace direct left/right recursion with Kleene operator](doc/refactoring.md#Kleene)
* Replace indirect left recursion with right recursion
* Replace parser rule symbols that conflict with Antlr keywords
* [Replace string literals in parser with lexer symbols](doc/refactoring.md#replace-literals-in-parser-with-lexer-token-symbols)
* Replace string literals in parser with lexer symbols, with lexer rule create

### Splitting and combining

* [Split combined grammars](doc/refactoring.md#splitting-and-combining-grammars)
* [Combine splitted grammars](doc/refactoring.md#splitting-and-combining-grammars)

## Conversion

* [Antlr3 import](doc/Import.md#antlr3)
* [Antlr2 import](doc/Import.md#antlr2)
* [Bison import](doc/Import.md#bison)

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

# Current Release v8.0 (18 Aug 2020)

This release is a major organizational and feature change.
For the extension itself, there are several new transforms 
and some bug fixes.
But, the main goal has been to reposition the tool to be used
for refactorings of the Java grammar from specification in
an automated way. To that end, I also wrote
[ScrapeJavaSpec](https://github.com/kaby76/ScrapeJavaSpec),
a tool that
scrapes the Java grammar directly from the online specification,
and outputs an Antlr4 grammar. This works wonderfully not only
for the existing grammars [Java8](https://github.com/antlr/grammars-v4/tree/master/java/java8),
[Java9](https://github.com/antlr/grammars-v4/tree/master/java/java9),
but also for versions 10, 11, 12, 13, and 14, in minutes!
Added with this release of Antlrvsix are several new refactorings,
like "unify", which I
have tested on the Java grammar.
The next step with the next release will be
to add the rest of the refactorings and
optimize the Java grammar for speed so that it is as fast as
[Parr's Java grammar](https://github.com/antlr/grammars-v4/tree/master/java/java).
Also added to this release were
conversions for Antlr2 and Antlr3 grammars. But, in order to write
the conversion methods, I had to write Antlr4 grammars for these older syntaxes.
Since it was not much trouble, I
decided it was not too much effort to also provide LSP services for
Antlr2, Antlr3, and Bison grammars!
The other major software that is new with this release is an XPath2
engine. This was a port of the Java code for the Eclipse XPath engine
to C#, and an added layer to wrap Antlr parse trees.

* Add editor support for Antlr2, Antlr3, and Bison grammars.
* Add convert Antlr2 and Antlr3 grammars to Antlr4.
* Add **Trash**, the **Tr**ansformation system for **A**ntlr **Sh**ell,
a completely command-line interface to apply transforms to a grammar,
for Windows or Linux.
  * Expose many of the current transforms in the **Trash** tool.
  * Add additional transform **unify**, **ulliteral**.
* Add XPath2 engine so as to identify Antlr parse tree nodes for transforms.
* Move all Antlr tree editing routines into a NuGet package
  * Add XPath based on Eclipse XPath2.
  * Add CTree, an S-expression library to specify and link in sub-trees.
  * Move replace and delete to this NuGet library.
  * Add observer pattern for parse tree node edits to keep XPath DOM in sync.
* Rewrite the existing transforms and analyses using XPath and S-expresions.
* Remove templates from extension and place in Antlr4BuildTasks.Templates.
* Add nightly unit tests to build (find def, find refs and defs, etc).
* Make signficant updates to the documentation.
* Fix [Import of grammar with multiple rules for LHS symbol crashes. #69](https://github.com/kaby76/AntlrVSIX/issues/69).
* Fix [Add transform to input string literals and convert to case insensitive literal or vice versa. #71](https://github.com/kaby76/AntlrVSIX/issues/71).
* Fix [LanguageServer.Module.GetDefsAndRefs() not working right #74](https://github.com/kaby76/AntlrVSIX/issues/74).
* Fix [Workspace.FindDocument() and Document.FindDocument() need to use normalized file paths. #75](https://github.com/kaby76/AntlrVSIX/issues/75).

# Prior Releases

See [this guide](PriorReleases.md).

# Roadmap

## Planned for v8.1 (Aug 2020):

* Add expression rule optimization.
* Add left factoring and inverse.
* Add in empty string alternative hoist transforms.

Any questions, email me at ken.domino <at> gmail.com
