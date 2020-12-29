# Prior Releases

## Release v8.2 VSIDE, v1.1 VSCode (5 Nov 2020)

This release addresses bug and performance issues in Antlrvsix.

Most of the bugs had to do with commands in Trash.
In the command-line interpreter, I wrote two special Antlr streams that were very buggy.
With this release, I rewrote this code to parse input
in two steps: the first step reads a line of text
until the end-of-line or end-of-file; the second step parses the line of text
with an Antlr parser using a simplified grammar (it no longer considers whitespace in
the parser grammar, and the lexer uses modes).

For several commands in Trash, I supported
[file globbing](https://en.wikipedia.org/wiki/Glob_(programming)),
e.g., `ls *.g4`.
However, I was never happy with the implementation because it was not very good.
The globbing code was a thin
layer over Microsoft's basic FileInfo and DirectoryInfo APIs,
which does a poor job at file pattern matching (e.g., one could not write `ls *Lex*.g4`,
which has two asterisks, but one could for `ls *Lexer.g4`).
I replaced this code with a Bash-like globbing library that
[I wrote](https://github.com/kaby76/AntlrVSIX/blob/4f54c980ae91cc32d32342c3a8d973b79aca925a/Trash/Globbing.cs).
File and directory names, however, are now case sensitive because the file names should be
more like Linux, since this tool is intended to be platform independent. In fact, I wrote [code](https://github.com/kaby76/AntlrVSIX/blob/5fba2752ea797de42896511d2fc9b4d4bc792c7c/Workspaces/Document.cs#L77)
long ago that takes great effort to mutate a name
that is in the wrong case to the proper case. It should not even be there because it continues
a non-portable feature. You will need to use the correct case for all file names.

Recently, I had a task to merge a couple of large Antlr grammars.
Some of the keyword
rules in one grammar were in case-folding syntax (e.g., `TRUE: [tT][rR][uU][eE];`),
while in the other grammar the rules were not (e.g., `TRUE: 'true';`). After
playing around with the [ulliteral transform](https://github.com/kaby76/AntlrVSIX/blob/master/doc-8.2/refactoring.md#upper-and-lower-case-string-literals),
I realized that it was not working all that well.
In addition, there was no inverse of the transform--which is
very useful because Antlr warns if a grammar
contains two lexer rules that match the same string literal, but not if one grammar
is imported by the other, and not if the lexer rule is in case-folding syntax. I added
'unulliteral' for these situations.

Probably the most exciting change to Trash is the introduction of pipes between commands,
similar to what you would see in Bash. Instead of passing a plain character buffer between
commands, though, I pass parse trees. So, you can do something like this

    read Expr.g4
    parse
    . | find //lexerRuleSpec/TOKEN_REF | text

to print out the lexer rule symbols. But, I have more things planned for the next few months.

The complete list of bugs fixed in this release are:

* Fix ["alias w=write" does not work #105](https://github.com/kaby76/AntlrVSIX/issues/105)
* Fix ["cd .." does not work #104](https://github.com/kaby76/AntlrVSIX/issues/104)
* Fix [Ulliteral should be able to handle non-uppercase and non-lowercase characters like '_' #103](https://github.com/kaby76/AntlrVSIX/issues/103)
* Partial fix [Antlr produces a warning for token rules that match the same string literal, but not for u/l cased defs #102](https://github.com/kaby76/AntlrVSIX/issues/102)
* Fix [ulliteral of a string with numbers gives sets with dups e.g., "2" => "[22]" #101](https://github.com/kaby76/AntlrVSIX/issues/101)
* Fix [Trash "foldlit //lexerRuleSpec/TOKEN_REF" really slow for PlSqlParser/Lexer.g4 #100](https://github.com/kaby76/AntlrVSIX/issues/100)
* Fix [Trash crashes if given eof, in script not given "quit" command #98](https://github.com/kaby76/AntlrVSIX/issues/98)
* Fix [Links to User Guide and Documentation are broken #97](https://github.com/kaby76/AntlrVSIX/issues/97)
* Fix [Performance Problems #96](https://github.com/kaby76/AntlrVSIX/issues/96)

## Release v8.1 (21 Oct 2020):

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
* Add Visual Studio Code client to Marketplace.
* Add Gnu Emacs extension.
* Add WC3 EBNF parsing and conversion.
* Add comment lines in Trash.
* Add Help command in Trash.
* Add Ungroup transform in Trash.
* Add Delabel transform in Trash.
* Replace [Microsoft.VisualStudio.LanguageServer.Protocol](https://www.nuget.org/packages/Microsoft.VisualStudio.LanguageServer.Protocol/) with a drop-in replacement in order to handle semantic highlighting.

# Release v8.0 (18 Aug 2020)

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

