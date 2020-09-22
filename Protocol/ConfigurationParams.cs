using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class ConfigurationParams
    {
        public ConfigurationParams() { }

        [DataMember(Name = "items")]
        [JsonProperty(Required = Required.Always)]
        public ConfigurationItem[] Items { get; set; }
    }
}
