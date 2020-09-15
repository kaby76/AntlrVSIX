using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a request sent from a language server to modify resources
    //     in the workspace.
    [DataContract]
    public class WorkspaceEdit
    {
        public WorkspaceEdit() { }

        //
        // Summary:
        //     Gets or sets a dictionary holding changes to existing resources
        [DataMember(Name = "changes")]
        public Dictionary<string, TextEdit[]> Changes { get; set; }
        //
        // Summary:
        //     Gets or sets an array representing versioned document changes
        [DataMember(Name = "documentChanges")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TextDocumentEdit[] DocumentChanges { get; set; }
    }
}
