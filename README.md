# AntlrVSIX
This is an alternative, open source Visual Studio 2015/2017 extension for Antlr version 4 grammars.

This extension is based on the OokLanguage extension (https://github.com/Microsoft/VSSDK-Extensibility-Samples/tree/master/Ook_Language_Integration and
also https://github.com/visual-studio-extension/ook-language).
If you want to customize this extension for your needs,
take care of the caveots.

* This extension only works on Antlr4 grammars, and the grammar must be in a file that has the suffix
".g4".

* If adding another token class, like punctuation, make sure to add the classification to:
AntlrTokenTagger; AntlrClassifier; ClassificationFormat.cs; OrdinaryClassificationDefinition.cs;
AugmentQuickInfoSession; AugmentCompletionSession. If you don't, you end up with very
bizarre errors in the ActivityLog.xml file for Visual Studio that are very hard to track down.
I suggest you do a search for "nonterminal" and observe all the locations you may have to
modify.

* The grammar used is the standard Antlr4 grammar in the examples: 
https://github.com/antlr/grammars-v4/tree/master/antlr4. There were some mofications to get it
to work in C#.

* Parsing of the input is not incremental, and currently does not recover from
syntax errors at all. If the input grammar does not parse, there is no mark up.

* You should reset your Experimental Hive for Visual Studio. To do that, execute from Cygwin (or a cmd, the
equivalent of):
  1. $ cd '/cygdrive/c/Program Files (x86)/Microsoft Visual Studio 14.0/VSSDK/VisualStudioIntegration/Tools/Bin'
  2. $ ./CreateExpInstance /Reset /VSInstance=14.0 /RootSuffix=Exp

* "Go to definition" and "Find all references" are implemented. This extension is NOT a
Language Service! As noted in _Legacy Language Service Extensibility_
(https://msdn.microsoft.com/en-us/library/bb165099.aspx) "Legacy language
services are implemented as part of a VSPackage, but the newer way to implement
language service features is to use MEF extensions." In addition, the code
that is typically employed is overly complicated, difficult to read, and very poorly
encapsulated.

Go To Definition or Find All References: After opening a ".g4" file, make sure the extension works
by noting tagging--comments should be green, nonterminals purple, terminals lime green,
and keywords blue. Right-click on a symbol, then select "go to definition" or
"find all references". For "go to definition", if the symbol is defined, it will open the file containing
the symbol (the file must be part of a project, or open already). If the symbol is
unknown, the operation will do nothing. For "find all references", the "Find All References"
window will diplay a list of {file name, line number, column number} records that
is a reference of the symbol selected. Click on a row, and the editor will open
the file and position the cursor to the reference. Note, the "Find All References"
window is located in "View -> Other windows -> FindAllReferences". You might want
to dock the window in Visual Studio for handy reference.
