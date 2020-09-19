using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum representing the reason a document was saved.
    [DataContract]
    public enum TextDocumentSaveReason
    {
        //
        // Summary:
        //     Save was manually triggered.
        Manual = 1,
        //
        // Summary:
        //     Save was automatic after some delay.
        AfterDelay = 2,
        //
        // Summary:
        //     Save was automatic after the editor lost focus.
        FocusOut = 3
    }
}
