using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter sent with workspace/didChangeConfiguration
    //     requests.
    [DataContract]
    public class DidChangeConfigurationParams
    {
        public DidChangeConfigurationParams() { }

        /**
	     * The actual changed settings
	     */
        [DataMember(Name = "settings")]
        [JsonProperty(Required = Required.Always)]
        public object Settings { get; set; }
    }
}
