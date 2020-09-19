using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the watcher options for Microsoft.VisualStudio.LanguageServer.Protocol.DidChangeWatchedFilesRegistrationOptions
    [DataContract]
    public class FileSystemWatcher
    {
        public FileSystemWatcher() { }

        //
        // Summary:
        //     Gets or sets the glob pattern to watch.
        [DataMember(Name = "globPattern")]
        public string GlobPattern { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.WatchKind values
        //     that are of interest.
        [DataMember(Name = "kind")]
        public WatchKind Kind { get; set; }
    }
}
