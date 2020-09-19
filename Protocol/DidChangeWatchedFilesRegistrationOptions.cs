using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for registering workspace/didChangeWatchedFiles
    //     support.
    [DataContract]
    public class DidChangeWatchedFilesRegistrationOptions
    {
        public DidChangeWatchedFilesRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets the watchers that should be registered.
        [DataMember(Name = "watchers")]
        public FileSystemWatcher[] Watchers { get; set; }
    }
}
