# AntlrVSIX
This is an alternative, open source Antlr4 Visual Studio 2015 extension. It only works
on Antlr4 files, which must have the suffix ".g4".

This extension is based on the OokLanguage extension. If you want to customize the extension
for your needs, take care to make sure you follow a few rules:

1) If adding another token class, like punctuation, make sure to add the classification to:
AntlrTokenTagger; AntlrClassifier; ClassificationFormat.cs; OrdinaryClassificationDefinition.cs;
AugmentQuickInfoSession; AugmentCompletionSession. If you don't, you end up with very
bizarre errors in the ActivityLog.xml file for Visual Studio that are very hard to track down.
I suggest you do a search for "nonterminal" and observe all the locations you may have to
modify.

2) The grammar used is the standard Antlr4 grammar in the examples: 
https://github.com/antlr/grammars-v4/tree/master/antlr4. There were some mofications to get it
to work in C#.

3) 