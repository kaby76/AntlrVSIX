using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //  Options to create a file.
    [DataContract]
    public class CreateFileOptions
    {
        public CreateFileOptions() { }

        // Overwrite existing file. Overwrite wins over `ignoreIfExists`
        [DataMember(Name = "overwrite")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Overwrite { get; set; }

        // Ignore if exists.
        [DataMember(Name = "ignoreIfExists")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IgnoreIfExists { get; set; }

    }
}
