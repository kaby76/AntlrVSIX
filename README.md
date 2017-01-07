# AntlrVSIX
This is an alternative, open source Visual Studio 2015/2017 extension for Antlr version 4 grammars.

This extension is based on the OokLanguage extension (https://github.com/Microsoft/VSSDK-Extensibility-Samples/tree/master/Ook_Language_Integration and
also https://github.com/visual-studio-extension/ook-language).
If you want to customize this extension for your needs,
take care of the caveots.

* This extension only works on Antlr4 grammars, and the grammar must be in a file that has the suffix ".g4".

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

