# AntlrVSIX
AntlrVSIX is an open source Visual Studio 2015 extension for Antlr version 4
grammars. The features in this extension are:

* Colorized tagging of grammars, including:
i. _Terminals_, which are symbols defined in the LHS of a lexical grammar production rule.
Terminals always have the first letter capitalized. These symbols are tagged in lime
green.
ii. _Nonterminals_, which are symbols defined in the LHS of a parser grammar production rule.
Nonterminals never have the first letter capitalized. These symbols are tagged in purple.
iii. _Comments_, which are either single line (_//_) or multiple line (_/* ... */_). These are
tagged in green.
iv. _Keywords_, such as _fragment_, _grammar_, etc. Keywords are tagged in blue.
v. _Literals_, such as _[\n\r ]_, _'foobar'_, etc. These are tagged in red.

* "Go to definition": AntlrVSIX can locate the rule that defines the symbol.
Right-click on a symbol, and choose "Go to definition" in the pop-up menu.</br>
<a href="http://www.youtube.com/watch?feature=player_embedded&v=Kl_qaY0NF70" target="_blank"><img src="http://img.youtube.com/vi/Kl_qaY0NF70/0.jpg" 
alt="Go to definition" width="240" height="180" border="10" /></a>

* "Find all references": AntlrVSIX can locate the defining and applied occurrences
of a symbol. Right-click on a symbol, and choose "Find all references" in the pop-up
menu. Open the "FindAllReferences" windows and select any occurrence.
</br>
<a href="http://www.youtube.com/watch?feature=player_embedded&v=wOX_T4LP8QU
" target="_blank"><img src="http://img.youtube.com/vi/wOX_T4LP8QU/0.jpg" 
alt="Go to definition" width="240" height="180" border="10" /></a>

* "Replace symbol": AntlrVSIX can rename the defining and applied occurrences of a
symbol. Right-click on a symbol, and choose "Rename Antlr symbol" in the pop-up
menu. In the pop-up modal dialog box, enter the new name, then click "OK". You must
manually save the files to make the changes permanent.
</br>
<a href="http://www.youtube.com/watch?feature=player_embedded&v=JJSQ2_fjxvc
" target="_blank"><img src="http://img.youtube.com/vi/JJSQ2_fjxvc/0.jpg" 
alt="Go to definition" width="240" height="180" border="10" /></a>

* "Reformat": AntlrVSIX can reformat the entire file using Codebuff, which is
 a machine-learning format tool (https://github.com/antlr/codebuff http://dl.acm.org/citation.cfm?id=2997383 https://arxiv.org/abs/1606.08866).
To reformat you're grammar, you will need to create a list of Antlr4 grammars, set the environmental
variable CORPUS_LOCATION to the directory. AntlrVSIX will read all ".g4" grammars in
the directory, the reformat the current document from the formatting discovered in the corpus.
You must manually save the file to make the changes permanent.
</br>
<a href="http://www.youtube.com/watch?feature=player_embedded&v=XPC-wxucdoU
" target="_blank"><img src="http://img.youtube.com/vi/XPC-wxucdoU/0.jpg" 
alt="Go to definition" width="240" height="180" border="10" /></a>

* Open all the grammar files in Visual Studio you wish to search, or add the
grammar files to a project/solution. AntlrVSIX will go through all files, parse,
and record the occurrences of every symbol.

* No advertisements will ever be displayed using this extension.

## Caveats:

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

* The grammar for Antlr that this extension uses may not be the "official" version for Antlr4. Consequently, your
grammar may be valid according to the Antlr compiler but not with this extension.
Please bear with me while I try to correct the grammar.

## Alternative Visual Studio Extensions

* ANTLR Language Support -- https://marketplace.visualstudio.com/items?itemName=SamHarwell.ANTLRLanguageSupport
* Antlr4Code -- https://marketplace.visualstudio.com/items?itemName=RamonFMendes.Antlr4Code
* Actipro SyntaxEditor for WPF -- https://marketplace.visualstudio.com/items?itemName=ActiproSoftware.ActiproSyntaxEditorforWPF
* Syntax Highlighting Pack -- https://marketplace.visualstudio.com/items?itemName=MadsKristensen.SyntaxHighlightingPack

