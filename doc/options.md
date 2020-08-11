# Options for Antlrvsix

AntlrVSIX stores optional settings in ~/.antlrvsixrc. This file,
in JSON format, has settings that affect the behavior of the VS2019 client
as well as the LSP server that it spawns. You can either edit the
file directly, or use VS2019 through "Extensions | Antlrvsix | Options...".

| Option | Type | Purpose | Default |
| ---- | ---- | ---- | ---- |
| IncrementalReformat | bool | _reserved_ | true |
| RestrictedDirectory | bool | | true |
| GenerateVisitorListener | bool | | false |
| OverrideAntlrPluggins | bool | | true |
| OptInLogging | bool | | false |
| CorpusLocation | string | | |
| AntlrNonterminalDef | string | | "type" |
| AntlrNonterminalRef | string | | "symbol definition" |
| AntlrTerminalDef | string | | "type" |
| AntlrTerminalRef | string | | "symbol definition" |
| AntlrComment | string | | "comment" |
| AntlrKeyword | string | | "keyword" |
| AntlrLiteral | string | | "string" |
| AntlrModeDef | string | | "type" |
| AntlrModeRef | string | | "" |
| AntlrChannelDef | string | | "" |
| AntlrChannelRef | string | | "field name" |
| AntlrPunctuation | string | | "field name" |
| AntlrOperator | string | | "field name" |
| VisibleServerWindow | bool | | true |
| EnableCompletion | bool | | false |

