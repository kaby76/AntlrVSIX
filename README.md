# AntlrVSIX

AntlrVSIX is an extension for Visual Studio 2019 to support editing, analysis, refactoring,
and conversion of Antlr2, Antlr3, Antlr4, and Bison grammars.
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
replace, and reformat. But, there is so much more to Antlrvsix
than these basic LSP commands. Along with the extension for VS,
[Trash](doc/Trash.md) is a command-line shell for
parsing, analyzing, and transforming grammars outside of the
Visual Studio extension.

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

## Analysis

When invoked in the UI, Antlrvsix will perform an analysis of the document and
make recommendations on what you may want to change. Results are place in the Error List grid box of VS 2019.
For performance, link to a special version of the Antlr library to perform analysis.

## Refactoring

Antlrvsix provides a number of transformations that can help to make grammars cleaner (reformatting),
more readable (reducing the length of the RHS of a rule),
and more efficient (reducing the number of non-terminals) for Antlr.

Some of these refactorings are very specific for Antlr due to the way
the parser works, e.g., converting a prioritized chain of productions recognizing
an arithmetic expression to a recursive alternate form.
The refactorings implemented are:

### Reordering

* Move start rule to top
* Reorder parser rules alphabetically
* Reorder parser rules BFS from start rule
* Reorder parser rules DFS from start rule
* Sort lexer modes alphabetically

### Changing rules

* [Delete parse tree node](doc/refactoring.md#delete-parse-tree-node)
* [Remove useless parentheses](doc/refactoring.md#remove-useless-productions)
* [Remove useless parser rules](doc/refactoring.md#remove-useless-productions)
* [Rename lexer or parser symbol](doc/refactoring.md#rename)
* [Unfold](doc/refactoring.md#Unfold)
* [Unify alts to EBNF](doc/refactoring.md#unify-alts-to-ebnf)
* [Upper and lower case string literals](doc/refactoring.md#upper-and-lower-case-string-literals)
* Add lexer rules for string literals in parser
* Fold
* Replace direct left recursion with right recursion
* Replace direct left/right recursion with Kleene operator
* Replace indirect left recursion with right recursion
* Replace parser rule symbols that conflict with Antlr keywords
* Replace string literals in parser with lexer symbols

### Splitting and combining

* [Split combined grammars](doc/refactoring.md#splitting-and-combining-grammars)
* Combine splitted grammars

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

# Roadmap

## Planned for v8.1 (Aug 2020):

* Add expression rule optimization.
* Add left factoring and inverse.

## Planned for v8.0 (expected mid-Aug 2020):

* Add editor support for Antlr2, Antlr3, and Bison grammars.
* Move all Antlr tree editing routines into a NuGet package
  * Add XPath based on Eclipse XPath2.
  * Add CTree, an S-expression library to specify and link in sub-trees.
  * Move replace and delete to this NuGet library.
  * Add observer pattern for parse tree node edits to keep XPath DOM in sync.
* Rewrite the existing transforms and analyses using XPath and S-expresions.
* Clean up parsing implementation for Antlr2, 3, 4, and Bison grammars.
Use XPaths to help label defs, grammar keywords, refs, punctuation,
comments, etc.
* Add in empty string alternative hoist transforms.
* Add nightly unit tests to build (find def, find refs and defs, etc).
* Antlr v2 and v3 conversion to Antlr v4.
* Add **Trash**, the **Tr**ansformation system for **A**ntlr **Sh**ell,
a completely command-line interface to apply transforms to a grammar,
for Windows or Linux.
* Fix [Import of grammar with multiple rules for LHS symbol crashes. #69](https://github.com/kaby76/AntlrVSIX/issues/69).
* Fix [Add transform to input string literals and convert to case insensitive literal or vice versa. #71](https://github.com/kaby76/AntlrVSIX/issues/71).
* Fix [LanguageServer.Module.GetDefsAndRefs() not working right #74](https://github.com/kaby76/AntlrVSIX/issues/74).
* Fix [Workspace.FindDocument() and Document.FindDocument() need to use normalized file paths. #75](https://github.com/kaby76/AntlrVSIX/issues/75).

# Current Release

## Release notes for v7.4 (7 Jun 2020):

* Add analysis:
  * Show cycles
  * Show useless lexer rules
* Fix [Add to about box the version of Antlr used. #63](https://github.com/kaby76/AntlrVSIX/issues/63).
* Fix [Add CI building of Antlrvsix. #61](https://github.com/kaby76/AntlrVSIX/issues/61).
* Fix ["Add lexer rules for string literals" adds rules but with the same lexer symbol name #60](https://github.com/kaby76/AntlrVSIX/issues/60).
* Fix [System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.' #58](https://github.com/kaby76/AntlrVSIX/issues/58).
* Fix [Eliminate direct left recursion should space new rules below last rule, retain old intertoken characters #56](https://github.com/kaby76/AntlrVSIX/issues/56).
 
# Prior Releases

See [this guide](PriorReleases.md).

Any questions, email me at ken.domino <at> gmail.com
