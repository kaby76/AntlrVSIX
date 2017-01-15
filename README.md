# AntlrVSIX
AntlrVSIX is an open source Visual Studio 2015 extension for Antlr version 4
grammars. The features in this extension are:

* Tagging of grammar symbols: terminals ("rule name" for lexical
rule; lime green), non-terminals ("rule name" for parser rules; purple),
comments (green), and keywords (blue).

* "Go to definition": AntlrVSIX can locate the rule that defines the symbol.
Right-click on a symbol, and choose "Go to definition" in the pop-up menu. Caveat:
if there are multiple rules that define the symbol, AntlrVSIX will go to the first.

* "Find all references": AntlrVSIX can locate the defining and applied occurrences
of a symbol. Right-click on a symbol, and choose "Find all references" in the pop-up
menu. Open the "FindAllReferences" windows and select any occurrence.

* Open all the grammar files in Visual Studio you wish to search, or add the
grammar files to a project/solution. AntlrVSIX will go through all files, parse,
and record the occurrences of every symbol.

Caveats:

* This extension only works on Antlr4 grammars, and the grammar must be in a file that has the suffix
".g4".

* If adding another token class, like punctuation, make sure to add the classification to:
AntlrTokenTagger; AntlrClassifier; ClassificationFormat.cs; OrdinaryClassificationDefinition.cs;
AugmentQuickInfoSession; AugmentCompletionSession. If you don't, you end up with very
bizarre errors in the ActivityLog.xml file for Visual Studio that are very hard to track down.
I suggest you do a search for "nonterminal" and observe all the locations you may have to
modify.

* The grammar used is the standard Antlr4 grammar in the examples: 
https://github.com/antlr/grammars-v4/tree/master/antlr4. There were some modifications to
get the parser to work in C#.

* The parser is not incremental. The parse does not recover from
syntax errors at all. If the input grammar does not parse, there is no tagging.

* If you want to make modifications for yourself, you should reset your
Experimental Hive for Visual Studio. To do that, execute from Cygwin (or a cmd, the
equivalent of):
  1. $ cd '/cygdrive/c/Program Files (x86)/Microsoft Visual Studio 14.0/VSSDK/VisualStudioIntegration/Tools/Bin'
  2. $ ./CreateExpInstance /Reset /VSInstance=14.0 /RootSuffix=Exp

* "Go to definition" and "Find all references" are not implemented as a
Language Service! As noted in _Legacy Language Service Extensibility_
(https://msdn.microsoft.com/en-us/library/bb165099.aspx) "Legacy language
services are implemented as part of a VSPackage, but the newer way to implement
language service features is to use MEF extensions." The alternative approach,
a Language Service, is undocumented, and the examples that I could find (PTVS)
are bloated and poorly structured. Rather than take weeks, if not months, to understand and implement,
I chose a very simple WPF implementation.

