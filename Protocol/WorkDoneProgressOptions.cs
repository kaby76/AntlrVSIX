using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkDoneProgressOptions
    {
        public WorkDoneProgressOptions() { }

        [DataMember(Name = "workDoneProgress")]
        [JsonProperty(Required = Required.Default)]
        public bool WorkDoneProgress { get; set; }
    }
}
