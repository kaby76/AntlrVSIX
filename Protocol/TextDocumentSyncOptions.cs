using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents configuration values indicating how text documents should
    //     be synced.
    [DataContract]
    public class TextDocumentSyncOptions
    {
        public TextDocumentSyncOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether open and close notifications are sent
        //     to the server.
        [DataMember(Name = "openClose")]
        public bool OpenClose { get; set; }
        //
        // Summary:
        //     Gets or sets the value indicating how text documents are synced with the server.
        [DataMember(Name = "change")]
        public TextDocumentSyncKind Change { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether 'will save' notifications are sent to
        //     the server.
        [DataMember(Name = "willSave")]
        public bool WillSave { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether 'will save until' notifications are sent
        //     to the server.
        [DataMember(Name = "willSaveWaitUntil")]
        public bool WillSaveWaitUntil { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether save notifications are sent to the server.
        [DataMember(Name = "save")]
        public SaveOptions Save { get; set; }
    }
}
