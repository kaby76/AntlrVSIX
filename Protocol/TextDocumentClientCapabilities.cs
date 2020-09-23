using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    /**
     * Text document specific client capabilities.
     */
    [DataContract]
    public class TextDocumentClientCapabilities
    {
        public TextDocumentClientCapabilities() { }

        [DataMember(Name = "synchronization")]
        [JsonProperty(Required = Required.Default)]
        public TextDocumentSyncClientCapabilities Synchronization { get; set; }

        /**
	     * Capabilities specific to the `textDocument/completion` request.
	     */
        [DataMember(Name = "completion")]
        [JsonProperty(Required = Required.Default)]
        public CompletionClientCapabilities Completion { get; set; }
        
        /**
	     * Capabilities specific to the `textDocument/hover` request.
	     */
        [DataMember(Name = "hover")]
        [JsonProperty(Required = Required.Default)]
        public HoverClientCapabilities Hover { get; set; }

        /**
	     * Capabilities specific to the `textDocument/signatureHelp` request.
	     */
        [DataMember(Name = "signatureHelp")]
        [JsonProperty(Required = Required.Default)]
        public SignatureHelpClientCapabilities SignatureHelp { get; set; }

        /**
          * Capabilities specific to the `textDocument/declaration` request.
          *
          * @since 3.14.0
          */
        [DataMember(Name = "declaration")]
        [JsonProperty(Required = Required.Default)]
        public DeclarationClientCapabilities Declaration { get; set; }

        /**
	     * Capabilities specific to the `textDocument/definition` request.
	     */
        [DataMember(Name = "definition")]
        [JsonProperty(Required = Required.Default)]
        public DefinitionClientCapabilities Definition { get; set; }

        /**
	     * Capabilities specific to the `textDocument/typeDefinition` request.
	     *
	     * @since 3.6.0
	     */
        [DataMember(Name = "typeDefinition")]
        [JsonProperty(Required = Required.Default)]
        public TypeDefinitionClientCapabilities TypeDefinition { get; set; }

        /**
	     * Capabilities specific to the `textDocument/implementation` request.
	     *
	     * @since 3.6.0
	     */
        [DataMember(Name = "implementation")]
        [JsonProperty(Required = Required.Default)]
        public ImplementationClientCapabilities Implementation { get; set; }

        /**
	     * Capabilities specific to the `textDocument/references` request.
	     */
        [DataMember(Name = "references")]
        [JsonProperty(Required = Required.Default)]
        public ReferenceClientCapabilities References { get; set; }

        /**
	     * Capabilities specific to the `textDocument/documentHighlight` request.
	     */
        [DataMember(Name = "documentHighlight")]
        [JsonProperty(Required = Required.Default)]
        public DocumentHighlightClientCapabilities DocumentHighlight { get; set; }

        /**
	     * Capabilities specific to the `textDocument/documentSymbol` request.
	     */
        [DataMember(Name = "documentSymbol")]
        [JsonProperty(Required = Required.Default)]
        public DocumentSymbolClientCapabilities DocumentSymbol { get; set; }

        /**
          * Capabilities specific to the `textDocument/codeAction` request.
          */
        [DataMember(Name = "codeAction")]
        [JsonProperty(Required = Required.Default)]
        public CodeActionClientCapabilities CodeAction { get; set; }

        /**
	     * Capabilities specific to the `textDocument/codeLens` request.
	     */
        [DataMember(Name = "codeLens")]
        [JsonProperty(Required = Required.Default)]
        public CodeLensClientCapabilities CodeLens { get; set; }

        /**
	     * Capabilities specific to the `textDocument/documentLink` request.
	     */
        [DataMember(Name = "documentLink")]
        [JsonProperty(Required = Required.Default)]
        public DocumentLinkClientCapabilities DocumentLink { get; set; }

        /**
	     * Capabilities specific to the `textDocument/documentColor` and the
	     * `textDocument/colorPresentation` request.
	     *
	     * @since 3.6.0
	     */
        [DataMember(Name = "colorProvider")]
        [JsonProperty(Required = Required.Default)]
        public DocumentColorClientCapabilities ColorProvider { get; set; }

        /**
	     * Capabilities specific to the `textDocument/formatting` request.
	     */
        [DataMember(Name = "formatting")]
        [JsonProperty(Required = Required.Default)]
        public DocumentFormattingClientCapabilities Formatting { get; set; }

        /**
	     * Capabilities specific to the `textDocument/rangeFormatting` request.
	     */
        [DataMember(Name = "rangeFormatting")]
        [JsonProperty(Required = Required.Default)]
        public DocumentRangeFormattingClientCapabilities RangeFormatting { get; set; }

        /** request.
	     * Capabilities specific to the `textDocument/onTypeFormatting` request.
	     */
        [DataMember(Name = "onTypeFormatting")]
        [JsonProperty(Required = Required.Default)]
        public DocumentOnTypeFormattingClientCapabilities OnTypeFormatting { get; set; }

        /**
	     * Capabilities specific to the `textDocument/rename` request.
	     */
        [DataMember(Name = "rename")]
        [JsonProperty(Required = Required.Default)]
        public RenameClientCapabilities Rename { get; set; }

        /**
         * Capabilities specific to the `textDocument/publishDiagnostics` notification.
         */
        [DataMember(Name = "publishDiagnostics")]
        [JsonProperty(Required = Required.Default)]
        public PublishDiagnosticsClientCapabilities PublishDiagnostics { get; set; }

        /**
	     * Capabilities specific to the `textDocument/foldingRange` request.
	     *
	     * @since 3.10.0
	     */
        [DataMember(Name = "foldingRange")]
        [JsonProperty(Required = Required.Default)]
        public FoldingRangeClientCapabilities FoldingRange { get; set; }

        /**
	     * Capabilities specific to the `textDocument/selectionRange` request.
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "selectionRange")]
        [JsonProperty(Required = Required.Default)]
        public SelectionRangeClientCapabilities SelectionRange { get; set; }

        /**
	     * Capabilities specific to the `textDocument/semanticTokens/*` requests.
	     *
	     * @since 3.16.0
	     */
        [DataMember(Name = "semanticTokens")]
        [JsonProperty(Required = Required.Default)]
        public SemanticTokensClientCapabilities SemanticTokens { get; set; }
    }
}
