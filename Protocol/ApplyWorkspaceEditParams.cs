using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent from a server to a client for the workspace/applyEdit
    //     request.
    [DataContract]
    public class ApplyWorkspaceEditParams
    {
        public ApplyWorkspaceEditParams() { }

        //
        // Summary:
        //     Gets or sets the label associated with this edit.
        [DataMember(Name = "label")]
        public string Label { get; set; }
        //
        // Summary:
        //     Gets or sets the edit to be applied to the workspace.
        [DataMember(Name = "edit")]
        public WorkspaceEdit Edit { get; set; }
    }
}
