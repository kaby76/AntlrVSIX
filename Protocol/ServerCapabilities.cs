using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents server capabilities.
    [DataContract]
    public class ServerCapabilities
    {
        public ServerCapabilities() { }

        //
        // Summary:
        //     Gets or sets the value which indicates how text document are synced.
        [DataMember(Name = "textDocumentSync")]
   //TODO     [JsonConverter(typeof(TextDocumentSyncConverter))]
        public TextDocumentSyncOptions TextDocumentSync { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if completions are supported.
        [DataMember(Name = "completionProvider")]
        public CompletionOptions CompletionProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether the server provides hover support.
        [DataMember(Name = "hoverProvider")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<bool, HoverOptions> HoverProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if signature help is supported.
        [DataMember(Name = "signatureHelpProvider")]
        public SignatureHelpOptions SignatureHelpProvider { get; set; }

        // MISSING declarationProvider.

        //
        // Summary:
        //     Gets or sets a value indicating whether go to definition is supported.
        [DataMember(Name = "definitionProvider")]
        public bool DefinitionProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether go to type definition is supported.
        [DataMember(Name = "typeDefinitionProvider")]
        public bool TypeDefinitionProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether go to implementation is supported.
        [DataMember(Name = "implementationProvider")]
        public bool ImplementationProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether find all references is supported.
        [DataMember(Name = "referencesProvider")]
        public bool ReferencesProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether the server supports document highlight.
        [DataMember(Name = "documentHighlightProvider")]
        public bool DocumentHighlightProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether document symbols are supported.
        [DataMember(Name = "documentSymbolProvider")]
        public bool DocumentSymbolProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether code actions are supported.
        [DataMember(Name = "codeActionProvider")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<bool, CodeActionOptions> CodeActionProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if code lens is supported.
        [DataMember(Name = "codeLensProvider")]
        public CodeLensOptions CodeLensProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if document link is supported.
        [DataMember(Name = "documentLinkProvider")]
        public DocumentLinkOptions DocumentLinkProvider { get; set; }

        // MISSING colorProvider

        //
        // Summary:
        //     Gets or sets a value indicating whether document formatting is supported.
        [DataMember(Name = "documentFormattingProvider")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<bool, DocumentFormattingOptions> DocumentFormattingProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether document range formatting is supported.
        [DataMember(Name = "documentRangeFormattingProvider")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<bool, DocumentRangeFormattingOptions> DocumentRangeFormattingProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if document on type formatting is supported.
        [DataMember(Name = "documentOnTypeFormattingProvider")]
        public DocumentOnTypeFormattingOptions DocumentOnTypeFormattingProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether rename is supported.
        [DataMember(Name = "renameProvider")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<bool, RenameOptions> RenameProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if folding range is supported.
        [DataMember(Name = "foldingRangeProvider")]
   //TODO     [JsonConverter(typeof(FoldingRangeOptionsConverter))]
        public FoldingRangeProviderOptions FoldingRangeProvider { get; set; }

        //
        // Summary:
        //     Gets or sets the value which indicates if execute command is supported.
        [DataMember(Name = "executeCommandProvider")]
        public ExecuteCommandOptions ExecuteCommandProvider { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether rename is supported.
        [DataMember(Name = "semanticTokensProvider")]
        public SemanticTokensOptions SemanticTokensProvider { get; set; }


        //
        // Summary:
        //     Gets or sets a value indicating whether workspace symbols are supported.
        [DataMember(Name = "workspaceSymbolProvider")]
        public bool WorkspaceSymbolProvider { get; set; }

        //
        // Summary:
        //     Gets or sets experimental server capabilities.
        [DataMember(Name = "experimental")]
        public object Experimental { get; set; }
    }
}