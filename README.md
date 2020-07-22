# AntlrVSIX

AntlrVSIX is an extension for Visual Studio 2019 to support editing, analysis, refactoring, and conversion
of Antlr v4 grammars.
It is implemented using Microsoft's [Language Server Protocol (LSP)](https://langserver.org/) 
[client](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Client/) and
[server](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/) APIs,
[Antlr](https://www.antlr.org/), [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks),
[XPath3.1](https://en.wikipedia.org/wiki/XPath)
and
[S-exppressions](https://en.wikipedia.org/wiki/S-expression)
for Antlr parse trees,
and a number of other tools.
Most of the extension is implemented in C#.
There is a client for VS Code, written in Typescript,
but I am deferring further development of the client until the server is more or less complete.
Supported are colorized tagging, hover, go to def, find all refs,
replace, command completion, reformat, and go to visitor/listener. But, there is so much more to Antlrvsix
than these basic LSP commands.

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

* Replace string literals in parser with lexer symbols.
* Remove useless parser rules.
* Move start rule to top.
* Reorder parser rules alphabetically.
* Reorder parser rules DFS from start rule.
* Reorder parser rules BFS from start rule.
* Split combined grammars.
* Combine splitted grammars.
* Replace direct left recursion with right recursion.
* Replace indirect left recursion with right recursion.
* Replace parser rule symbols that conflict with Antlr keywords.
* Add lexer rules for string literals in parser.
* Sort lexer modes alphabetically.
* Replace direct left/right recursion with Kleene operator.
* Unfold.
* Fold.
* Remove useless parentheses.

## Conversion

* Bison import.

---------

The source code for the extension is open source, free of charge, and free of ads. For the latest developments on the extension,
check out my [blog](http://codinggorilla.com).

# Installation of Prerequisites

There are no prerequsites for Antlrvsix other than you use Visual Studio 2019.
I recommend that you use version 16.6 since it has the latest bug fixes.

If you are building and running an Antlr application--as opposed to
simply opening an Antlr grammar file to view--then you will want to set up the build environment. There is support for
Antlr C# programs using Antlr4BuildTasks. (Note, you can also develop Antlr programs for other languages that VS2019 supports.)
If you plan to build Antlrvsix, then you must set up the build environment
as described in the instructions of [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks).

# Installation

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

For information on how to use AntlrVSIX, see the [User Guide](doc/readme.md).

# Building Antlrvsix:

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

## Planned for v8.1 (Aug 2020):

* Add expression rule optimization.
* Add left factoring and inverse.

## Planned for v8.0 (expected by 2 Aug 2020):

* Add in XPath and S-expresion tree tools for less verbose
tree rewrite code.
* Rewrite some of the existing transforms and analyses
using XPath and S-expresions.
* Add in empty string alternative hoist transforms.
* Add nightly unit tests to build (find def, find refs and defs, etc).
* Antlr v1, v2, and v3 conversion to Antlr v4.
* Fix [Import of grammar with multiple rules for LHS symbol crashes. #69](https://github.com/kaby76/AntlrVSIX/issues/69).
* Fix [Add transform to input string literals and convert to case insensitive literal or vice versa. #71](https://github.com/kaby76/AntlrVSIX/issues/71).
* Fix [LanguageServer.Module.GetDefsAndRefs() not working right #74](https://github.com/kaby76/AntlrVSIX/issues/74).
* Fix [Workspace.FindDocument() and Document.FindDocument() need to use normalized file paths. #75](https://github.com/kaby76/AntlrVSIX/issues/75).

## Release notes for v7.4 (7 Jun 2020):

* Add analysis:
  * Show cycles
  * Show useless lexer rules
* Fix [Add to about box the version of Antlr used. #63](https://github.com/kaby76/AntlrVSIX/issues/63).
* Fix [Add CI building of Antlrvsix. #61](https://github.com/kaby76/AntlrVSIX/issues/61).
* Fix ["Add lexer rules for string literals" adds rules but with the same lexer symbol name #60](https://github.com/kaby76/AntlrVSIX/issues/60).
* Fix [System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.' #58](https://github.com/kaby76/AntlrVSIX/issues/58).
* Fix [Eliminate direct left recursion should space new rules below last rule, retain old intertoken characters #56](https://github.com/kaby76/AntlrVSIX/issues/56).
  
## Release notes for v7.3 (31 May 2020):

* New refactorings:
  * Fold/Pack
  * Unfold
  * Useless parentheses removal
* Fix [Clean up the 400+ compiler warnings #51](https://github.com/kaby76/AntlrVSIX/issues/51).
* Fix [Rename via right-click context menu does not work--VS 2019 broken, MS needs to fix. #48](https://github.com/kaby76/AntlrVSIX/issues/48).
* Fix [Output message when operation does nothing. #46](https://github.com/kaby76/AntlrVSIX/issues/46).
* Fix [LanguageServer is net472, should be netstandard2.0 #45](https://github.com/kaby76/AntlrVSIX/issues/45).
* Fix [BFS ordering not working with multiple start states; Start symbols not computed correctly for LeftFactored grammar. #44](https://github.com/kaby76/AntlrVSIX/issues/44).
* Partial fix [Colorizing of text after transformation is messed up. #43](https://github.com/kaby76/AntlrVSIX/issues/43). Colorization after undo still messed up.
* Fix [Add in UI to indicate if transform doesn't work and why #41](https://github.com/kaby76/AntlrVSIX/issues/41).
* Partial fix [Replace all hacks in transformations with tree editing #40](https://github.com/kaby76/AntlrVSIX/issues/40). Deferring all additional hacks with integration of Piggy, which will replace everything.

## Release notes for v7.2 (6-May-2020):

* Fix [Remote language server is not working until identity is checked #39](https://github.com/kaby76/AntlrVSIX/issues/39).
* Fix [Options window not working #38](https://github.com/kaby76/AntlrVSIX/issues/38).
* (Note, the UI contains the Fold transformation, but it is just a stub.)

## Release notes for v7.0 (4-May-2020):

* Added transforms for the elimination of left recursion.
* Added transform to convert left or right recursion to Kleene form.
* Expand tagging of all parts of grammar file.
* Changed colorizing text to use Visual Studio
character classes, with a mapping from Antlr classes to VS classes in the options box.

## Release notes for v6.0.1 (13-Apr-2020):

* Fix colorization after editing.

## Release notes for v6.0 (11-Apr-2020):

* Added selection of colors for grammar symbols in options. (I am now using Dark theme in Visual Studio 2019.)
* Changed options to use Newtonsoft.Json (breaking change--remove ~/.antlrvsixrc).
* Added refactoring to sort mode sections in lexer grammars.
* Added code to abbreviate action blocks in hover tool tips.
* Added code to allow multiple definitions of a symbol. This can happen 
for lexer symbols defined in a tokens section and as a lexer rule.
* Bug fixes.

## Release notes for v5.8 (29-Mar-2020):

* Added in rename on Antlrvsix tool menu because Microsoft broke LSP rename!

## Release notes for v5.7 (29-Mar-2020):

* Disabled completion since this is crashing the server. An option to enable was added so it can be turned on when fixed.

## Release notes for v5.6 (28-Mar-2020):

* Import of Bison/Yacc grammars. This will go well beyond anything that was implemented before (e.g.,
[Bison to ANTLR translator](https://www.antlr3.org/share/list) by Parr and Cramer 2006 for Antlr3 conversions),
but will stop far short of what I would like.
It will rewrite terminals and non-terminals to suitable Antlr symbols,
folding of string literals immediately as lexer rules,
and declare terminals as tokens in the lexer grammar.
Additional fixes may be available if I get more transformations working.
But, there is so much I can't do at this point until more infrastructure is set up.
* Bug fixes.

## Release notes for v5.5 (19-Mar-2020):

* Added in more refactorings:
a) Sort rules alphabetically, DFS or BFS traversals.
b) Separate/combine grammars.
* Bug fixes.

## Release notes for v5.4 (11-Mar-2020):

* Added in a few refactoring transformations (remove useless parser productions, convert parser string literals to
lexer token symbols, move start rule to top of grammar). _Note: Lexer rules are prioritized, so transformations on these
types may not be totally correct at the moment._
* Fixed goto visitor/listener.
* Added stability fixes.

## Release notes for v5.3 (5-Mar-2020):

* Fixed "Find references" of grammar symbols when opening only lexer grammar file.
* Fixed Format Document.
* Fixed Go To Listener/Visitor. _Caveat: only works for C# and you must save solution before using._
* Templates updated, Antlr4BuildTasks 2.2.

## Release notes for v5.2 (16-Feb-2020):

* Re-added About Box, Options Box, and next/previous grammar symbol.
* Options are now contained in ~/.antlrvsixrc, a JSON file.

## Release notes for v5.1 (7-Feb-2020):

* Re-added colorized tagging of grammar. When I switch to the LSP implementation, this functionality was lost because it's not
directly supported by the LSP Client/Server API that Microsoft provides. Microsoft says to implement this functionality using
TextMate files, but that is not how it should be done--it duplicates the purpose of the LSP server, and it's hard to
get right. Instead, it's implemented with a Visual Studio ITagger<> and a custom message to the LSP server.

## Release notes for v5.0 (25-Jan-2020):

* Restructuring the code as a Language Server Protocol client/server implementation with extensions for VS 2019 (IDE) and VS Code.

* Templates for C# and C++. Note, Antlr 4.8 currently does not have a C++ pre-built binary release for Windows. You will need to build the
runtime and update the generated .vcxproj file with path information. The template will generate a project that is expecting Debug Static and x64 target.

* These are the LSP features currently implemented. Note,
[Midrosoft.VisualStudio.LanguageServer.Protocol](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/)
version 16.4.30 does not implement LSP version 3.14, rather something around version 3.6. What is missing is color tagging.
I'm not sure how to deal with this other than pitch the inferior API, then use OmniSharp's API, or as usual write everything
myself.

* Computing the completion symbols is not complete. It only gives token types for completion, not the actual symbols that could
be inserted.

| Message  | Support |
| ---- | ---- |
| General | |
| initialize | yes (Dec 10, '19) |
| initiialized | yes (Dec 10, '19) |
| shutdown | yes (Dec 10, '19) |
| exit | yes (Dec 10, '19) |
| $/cancelRequest | no |
| ---- | ---- |
| Window | |
| window/showMessage | no |
| window/showMessageRequest | no |
| window/logMessage | no |
| ---- | ---- |
| Telemetry | |
| telemetry/event | no |
| ---- | ---- |
| Client | |
| client/registerCapability | no |
| client/unregisterCapability | no |
| ---- | ---- |
| Workspace | |
| workspace/workspaceFolders | no |
| workspace/didChangeWorkspaceFolders | no |
| workspace/didChangeConfiguration | no |
| workspace/configuration | no |
| workspace/didChangeWatchedFiles | no |
| workspace/symbol | no |
| workspace/executeCommand | no |
| workspace/applyEdit | no |
| ---- | ---- |
| Text Synchronization | |
| textDocument/didOpen | yes (Dec 10, '19) |
| textDocument/didChange | yes (Dec 15, '19) |
| textDocument/willSave | yes (Dec 18, '19) |
| textDocument/willSaveWaitUntil | yes (Dec 18, '19) |
| textDocument/didSave | yes (Dec 18, '19) |
| textDocument/didClose | yes (Dec 18, '19) |
| ---- | ---- |
| Diagnostics | |
| textDocument/publishDiagnostics | yes (Dec 10, '19) |
| ---- | ---- |
| Language Features | |
| textDocument/completion | no |
| completionItem/resolve | no |
| textDocument/hover | yes (Dec 10, '19) |
| textDocument/signatureHelp | no |
| textDocument/declaration | unavailable in API |
| textDocument/definition | yes (Dec 11, '19) |
| textDocument/typeDefinition | yes (same as "definition") |
| textDocument/implementation | yes (same as "definition") |
| textDocument/references | yes (Dec 11, '19) |
| textDocument/documentHighlight | yes (Dec 11, '19) |
| textDocument/documentSymbol | yes (Dec 10, '19) |
| textDocument/codeAction | no |
| textDocument/codeLens | no |
| codeLens/resolve | no |
| textDocument/documentLink | no |
| documentLink/resolve | no |
| textDocument/documentColor | unavailable in API |
| textDocument/colorPresentation | unavailable in API |
| textDocument/formatting | yes (Dec 17, '19) |
| textDocument/rangeFormatting | no |
| textDocument/onTypeFormatting | no |
| textDocument/rename | yes (Dec 18, '19) |
| textDocument/prepareRename | unavailable in API |
| textDocument/foldingRange | no |

## Release notes for v4.0.5 (12-Nov-2019):

* Fixing #26.

## Release notes for v4.0.4 (12-Nov-2019):

* Fixing solution loading crash.
* Removing parse tree print code from Python--it was used for debugging and should not have been there.
* Stubbing out Python and Rust targets because they could interfere with (currently) much better extensions. Will add them
back once they are up to snuff.

A shout out to all those folks using this extension and have opted-in to the automatic reporting!!

## Release notes for v4.0.3 (11-Nov-2019):

* Restructuring the code for options.
* Adding in reporting of caught exceptions to server.
* Some changes for the stability for Java soruce.

## Release notes for v4.0.2 (1-Nov-2019):

* Updated symbol table with new classes to represent files, directories, and search paths. This will help
support for better modeling of Java's ClassPath, Antlr's imports, etc., so that scopes can be cleared out quickly and
easily when the sources have changed.
* Fixing issues in stability with Antlr and Java files.
* Fixing https://github.com/kaby76/AntlrVSIX/issues/23
* Fixing tool tips and highlighting for Java.
* Updated symbol table to allow ambigous code editing.
* Changing About and Options boxes to non-modal.

## Release notes for v4.0.1 (28-Oct-2019):

* Fixing stability issues with Antlr and Java files.

## Release notes for v4.0 (26-Oct-2019):

* Major changes to architecture, focusing on separation of GUI from a backend that works like Language Server Protocol (LSP).
* In the GUI, tagging was improved in quality and speed.
* In the backend, a symbol table was added to represent all symbols in a project and solution.
* For Antlr grammars, imports and tokenVocab are now used to determine the scope of the symbol table.
* For Antlr grammars, Intellisense pop-ups now give the rule definition and location of the file defining the symbol.
* No specific improvements were made for Python, Rust, and Java support, in favor of focusing on the improved overall design and implementation of the extension.

## Release notes for v3.0 (28-Aug-2019):

* Supports Java, Python, Rust, Antlr in various stages. Description of languages abstracted into a "Grammar Description".

## Release notes for v2.0.6 (13-Aug-2019):

* The extension will support VS 2019 and VS 2017.

* A menu for the extension will be added to a submenu under Extensions. The functionality provided will
duplicate that in context menus.

* The source code build files will be updated and migrated to the most recent version
of .csproj format that is compatible with VS extensions. Unfortunately, updating to the latest
(version 16) is not possible.

* With my [Antlr4BuildTasks](https://www.nuget.org/packages/Antlr4BuildTasks/) NuGet package,
.g4 files can be automatically compiled to .cs input within
VS 2019 without having to manually run the Antlr4 Java tool on the command line.
Building of the extension itself will be upgraded to use the Antlr4BuildTasks package.

* Listener and Visitor classes are generated for a grammar with a right-click
menu operation. For Listeners, there are two methods associated with a nonterminal.
Depress the control key to select the Exit method, otherwise it will
select the Enter method.

* An options menu is provided to turn on incremental parsing. By default, incremental
* parsing is off because it is very slow.

## Release notes for v1.2.4 (11-Mar-2017):

* The extension is now both VS 2017 and 2015 compatible.

* The results windows of Antlr Find All References is now "Antlr Find Results".

## Release notes for v1.2.3 (7-Mar-2017):

* Color selection through VS Options/Environment/Fonts and Colors. Look for "Antlr ..." named items.

* Bug fixes with Context Menu entries for AntlrVSIX. AntlrVSIX commands are now only visible when cursor positioned at an Antlr symbol in the grammar. This fixes the segv's when selecting AntlrVSIX commands in non-Antlr files.

## Release notes for v1.2.2 (10-Feb-2017):

## Release notes for v1.2 (31-Jan-2017):

## Release notes for v1.1.1 (18-Jan-2017):

## Release notes for v1.1 (17-Jan-2017):

## Release notes for v1.0 (17-Jan-2017):



Any questions, email me at ken.domino <at> gmail.com
