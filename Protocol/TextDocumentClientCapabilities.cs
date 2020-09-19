using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents text document capabilities.
    [DataContract]
    public class TextDocumentClientCapabilities
    {
        public TextDocumentClientCapabilities() { }

        //
        // Summary:
        //     Gets or sets the setting which determines if rename can be dynamically registered.
        [DataMember(Name = "rename")]
        public DynamicRegistrationSetting Rename { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if document link can be dynamically
        //     registered.
        [DataMember(Name = "documentLink")]
        public DynamicRegistrationSetting DocumentLink { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if code lens can be dynamically registered.
        [DataMember(Name = "codeLens")]
        public DynamicRegistrationSetting CodeLens { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if code action can be dynamically registered.
        [DataMember(Name = "codeAction")]
        public CodeActionSetting CodeAction { get; set; }
        //
        // Summary:
        //     Gets or sets the settings which determines if type definition can be dynamically
        //     registered.
        [DataMember(Name = "typeDefinition")]
        public DynamicRegistrationSetting TypeDefinition { get; set; }
        //
        // Summary:
        //     Gets or sets the settings which determines if implementation can be dynamically
        //     registered.
        [DataMember(Name = "implementation")]
        public DynamicRegistrationSetting Implementation { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if definition can be dynamically registered.
        [DataMember(Name = "definition")]
        public DynamicRegistrationSetting Definition { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if on type formatting can be dynamically
        //     registered.
        [DataMember(Name = "onTypeFormatting")]
        public DynamicRegistrationSetting OnTypeFormatting { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if range formatting can be dynamically
        //     registered.
        [DataMember(Name = "rangeFormatting")]
        public DynamicRegistrationSetting RangeFormatting { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if formatting can be dynamically registered.
        [DataMember(Name = "formatting")]
        public DynamicRegistrationSetting Formatting { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if document symbol can be dynamically
        //     registered.
        [DataMember(Name = "documentSymbol")]
        public DocumentSymbolSetting DocumentSymbol { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if document highlight can be dynamically
        //     registered.
        [DataMember(Name = "documentHighlight")]
        public DynamicRegistrationSetting DocumentHighlight { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if references can be dynamically registered.
        [DataMember(Name = "references")]
        public DynamicRegistrationSetting References { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if signature help can be dynamically
        //     registered.
        [DataMember(Name = "signatureHelp")]
        public SignatureHelpSetting SignatureHelp { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if hover can be dynamically registered.
        [DataMember(Name = "hover")]
        public HoverSetting Hover { get; set; }
        //
        // Summary:
        //     Gets or sets the completion setting.
        [DataMember(Name = "completion")]
        public CompletionSetting Completion { get; set; }
        //
        // Summary:
        //     Gets or sets the synchronization setting.
        [DataMember(Name = "synchronization")]
        public SynchronizationSetting Synchronization { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines how folding range is supported.
        [DataMember(Name = "foldingRange")]
        public FoldingRangeSetting FoldingRange { get; set; }
        //
        // Summary:
        //     Gets or sets the setting publish diagnostics setting
        [DataMember(Name = "publishDiagnostics")]
        public PublishDiagnosticsSetting PublishDiagnostics { get; set; }
    }
}
