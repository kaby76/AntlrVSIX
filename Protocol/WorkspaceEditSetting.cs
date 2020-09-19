using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents initialization settings for workspace edit.
    [DataContract]
    public class WorkspaceEditSetting
    {
        public WorkspaceEditSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether document changes event is supported.
        [DataMember(Name = "documentChanges")]
        public bool DocumentChanges { get; set; }
    }
}
