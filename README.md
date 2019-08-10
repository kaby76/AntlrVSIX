
# AntlrVSIX
AntlrVSIX is an open source Visual Studio 2019 extension for Antlr v4
grammars. This extension provides tagging, "go to def", "find all refs",
replace, "go to visitor/listener", command completion, options,
reformat, and project templates for compiling and running.
The source code is open source, free of charge, and free of ads.

For integration of build rules into VS2019 (i.e., so you do not
need to manually call the command-line tool to generate Antlr4 lexers and parsers),
please use [Antlr4BuildTasks](https://github.com/kaby76/Antlr4BuildTasks). If you
generate the grammar using one of the Antlr project templates, it is
added to the .CSPROJ file for you.

## Documentation

For information on how to use AntlrVSIX, see the [User Guide](doc/readme.md).

## Caveats:

* Support for VS2015 and older editions has been removed.
If you are interested in those, you can try using an older version of the extension.

* This extension only works on Antlr4 grammars, and the grammar must be in a file that has the suffix
".g4" or ".g"

* The grammar used is the standard Antlr4 grammar in the examples: 
https://github.com/antlr/grammars-v4/tree/master/antlr4.

* If you want to make modifications for yourself, you should [reset your
Experimental Hive for Visual Studio](https://docs.microsoft.com/en-us/visualstudio/extensibility/the-experimental-instance?view=vs-2017). To do that,
Microsoft recommends using CreateExpInstance.exe.
Unfortunately, I've found CreateExpInstance doesn't always work because it copies from
previous hives stored under the AppData directory. It is often easier to
just recursively delete all directories ...\AppData\Local\Microsoft\VisualStudio\16.0_*.

* The grammar for Antlr that this extension uses may not be the "official" version for Antlr4 because
it is version dependent, and I use a copy for this tool.
Consequently, your grammar may be valid according to the Antlr compiler but not with this extension.

* Use Visual Studio 2019 to build the extension.

## New in v2.0 (to be released):

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

## New in v1.2.4:

* The extension is now both VS 2017 and 2015 compatible.

* The results windows of Antlr Find All References is now "Antlr Find Results".

## New in v1.2.3:

* Color selection through VS Options/Environment/Fonts and Colors. Look for "Antlr ..." named items.

* Bug fixes with Context Menu entries for AntlrVSIX. AntlrVSIX commands are now only visible when cursor positioned at an Antlr symbol in the grammar. This fixes the segv's when selecting AntlrVSIX commands in non-Antlr files.

Any questions, email me at ken.domino <at> gmail.com

## Alternative Visual Studio Extensions

There are, of course, alternative extensions for Antlr. Feel free to check
them out. However, I think you will find this extension and Antlr4BuildTasks
the most advance of the bunch.

* ANTLR Language Support -- https://marketplace.visualstudio.com/items?itemName=SamHarwell.ANTLRLanguageSupport
* Antlr4Code -- https://marketplace.visualstudio.com/items?itemName=RamonFMendes.Antlr4Code
* Actipro SyntaxEditor for WPF -- https://marketplace.visualstudio.com/items?itemName=ActiproSoftware.ActiproSyntaxEditorforWPF
* Syntax Highlighting Pack -- https://marketplace.visualstudio.com/items?itemName=MadsKristensen.SyntaxHighlightingPack

You also might want to check out the
plug-in [ANTLR4 grammar syntax support](https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4)
for Visual Studio Code.
This tool offers similar functionality to AntlrVSIX, plus code completion,
parse tree visualization, railroad diagrams,
and a grammar call graph. Debugging of the generated parsers and lexers
are intrinsically supported by VS2019. I do not find railroad diagrams and call graph diagrams useful in
real grammar development, so I probably won't add those features to AntlrVSIX.


