using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with workspace/didChangeWatchedFiles
    //     message.
    [DataContract]
    public class DidChangeWatchedFilesParams
    {
        public DidChangeWatchedFilesParams() { }

        //
        // Summary:
        //     Gets or sets of the collection of file change events.
        [DataMember(Name = "changes")]
        public FileEvent[] Changes { get; set; }
    }
}
