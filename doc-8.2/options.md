# Options for Antlrvsix

AntlrVSIX stores optional settings in ~/.antlrvsixrc. This file,
in JSON format, has settings that affect the behavior of the VS2019 client
as well as the LSP server that it spawns. You can either edit the
file directly, or use VS2019 through "Extensions | Antlrvsix | Options...".

| Option | Type | Purpose | Default |
| ---- | ---- | ---- | ---- |
| IncrementalReformat | bool | _reserved_ | true |
| RestrictedDirectory | bool | _reserved_ | true |
| GenerateVisitorListener | bool | On _Go to Visitor/Listener_, if the extension does not find an implementation for visitor/listener, the extension will generate one if this option is set to _true_. | false |
| OverrideAntlrPluggins | bool | If you have other extensions installed to handle grammars and don't want Antlrvsix to format them, set this option to false. Otherwise, Antlrvsix will take over all formatting of the file in the editor. | true |
| OptInLogging | bool | When there is a crash, you can log the crash automatically to a server that keeps track of these for bug fixing, by setting this option to true. | false |
| CorpusLocation | string | The value of this option is the location of grammar files used for ML training for the reformat code. | Reads env. var. CORPUS_LOCATION. |
| AntlrNonterminalDef | string | This value specifies how parser rule symbol defs are colorized. | "type" |
| AntlrNonterminalRef | string | This value specifies how parser rule symbol refs are colorized. | "symbol definition" |
| AntlrTerminalDef | string | This value specifies how lexer rule symbol defs are colorized. | "type" |
| AntlrTerminalRef | string | This value specifies how lexer rule symbol refs are colorized. | "symbol definition" |
| AntlrComment | string | This value specifies how comments are colorized. | "comment" |
| AntlrKeyword | string | This value specifies how keywords are colorized. | "keyword" |
| AntlrLiteral | string | This value specifies how string, character, and numbers are colorized. | "string" |
| AntlrModeDef | string | This value specifies how lexer mode symbol names are colorized. | "type" |
| AntlrModeRef | string | This value specifies how lexer mode symbol name references are colorized. | "" |
| AntlrChannelDef | string | | "" |
| AntlrChannelRef | string | | "field name" |
| AntlrPunctuation | string | This value specifies how punctuation is colorized. | "field name" |
| AntlrOperator | string | This value specifies how operators are colorized. | "field name" |
| VisibleServerWindow | bool | This value specifies whether Antlrvsix should spawn a visible window for the server. This option is useful to see if the server is working, or to restart from scratch if you kill it. | true |
| EnableCompletion | bool | This option enables or disables completion recommendations from the server while typing. | false |

