using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    // Delete file options
    [DataContract]
    public class DeleteFileOptions
    {
		public DeleteFileOptions() { }

        // Delete the content recursively if a folder is denoted.
        [DataMember(Name = "recursive")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Recursive { get; set; }

        // Ignore the operation if the file doesn't exist.
        [DataMember(Name = "ignoreIfNotExists")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IgnoreIfNotExists { get; set; }
    }
}
