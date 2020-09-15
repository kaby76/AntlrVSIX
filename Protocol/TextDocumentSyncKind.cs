using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various ways to sync text documents.
    [DataContract]
    public enum TextDocumentSyncKind
    {
        //
        // Summary:
        //     Documents should not be synced at all.
        None = 0,
        //
        // Summary:
        //     Documents are synced by always sending the full text.
        Full = 1,
        //
        // Summary:
        //     Documents are synced by sending only incremental updates.
        Incremental = 2
    }
}
