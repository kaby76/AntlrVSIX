using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents workspace capabilities.
    [DataContract]
    public class WorkspaceClientCapabilities
    {
        public WorkspaceClientCapabilities() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether apply edit is supported.
        [DataMember(Name = "applyEdit")]
        public bool ApplyEdit { get; set; }
        //
        // Summary:
        //     Gets or sets the workspace edit setting.
        [DataMember(Name = "workspaceEdit")]
        public WorkspaceEditSetting WorkspaceEdit { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if did change configuration can be
        //     dynamically registered.
        [DataMember(Name = "didChangeConfiguration")]
        public DynamicRegistrationSetting DidChangeConfiguration { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if did change watched files can be
        //     dynamically registered.
        [DataMember(Name = "didChangeWatchedFiles")]
        public DynamicRegistrationSetting DidChangeWatchedFiles { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if symbols can be dynamically registered.
        [DataMember(Name = "symbol")]
        public SymbolSetting Symbol { get; set; }
        //
        // Summary:
        //     Gets or sets the setting which determines if execute command can be dynamically
        //     registered.
        [DataMember(Name = "executeCommand")]
        public DynamicRegistrationSetting ExecuteCommand { get; set; }
    }
}
