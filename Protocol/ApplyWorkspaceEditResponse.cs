using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the response sent for a workspace/applyEdit request.
    [DataContract]
    public class ApplyWorkspaceEditResponse
    {
        public ApplyWorkspaceEditResponse() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether edits were applied or not.
        [DataMember(Name = "applied")]
        public bool Applied { get; set; }
    }
}
