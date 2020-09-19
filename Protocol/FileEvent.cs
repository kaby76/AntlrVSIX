using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a file change event.
    [DataContract]
    public class FileEvent
    {
        public FileEvent() { }

        //
        // Summary:
        //     Gets or sets the URI of the file.
        [DataMember(Name = "uri")]
      //TODO  [JsonConverter(typeof(DocumentUriConverter))]
        public string Uri { get; set; }
        //
        // Summary:
        //     Gets or sets the file change type.
        [DataMember(Name = "type")]
        public FileChangeType FileChangeType { get; set; }
    }
}
