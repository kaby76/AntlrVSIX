using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents client capabilities.
    [DataContract]
    public class ClientCapabilities
    {
        public ClientCapabilities() { }

        //
        // Summary:
        //     Gets or sets the workspace capabilities.
        [DataMember(Name = "workspace")]
        public WorkspaceClientCapabilities Workspace { get; set; }
        //
        // Summary:
        //     Gets or sets the text document capabilities.
        [DataMember(Name = "textDocument")]
        public TextDocumentClientCapabilities TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the experimental capabilities.
        [DataMember(Name = "experimental")]
        public object Experimental { get; set; }
    }
}
