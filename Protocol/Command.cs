using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a reference to a command
    [DataContract]
    public class Command
    {
        public Command() { }

        //
        // Summary:
        //     Gets or sets the title of the command.
        [DataMember(Name = "title")]
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }
        //
        // Summary:
        //     Gets or sets the identifier associated with the command.
        [DataMember(Name = "command")]
        [JsonProperty(Required = Required.Always)]
        public string CommandIdentifier { get; set; }
        //
        // Summary:
        //     Gets or sets the arguments that the command should be invoked with.
        [DataMember(Name = "arguments")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object[] Arguments { get; set; }
    }
}
