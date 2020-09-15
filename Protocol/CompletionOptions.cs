using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents completion capabilities.
    [DataContract]
    public class CompletionOptions
    {
        public CompletionOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether server provides completion item resolve
        //     capabilities.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating all the possible commit characters associated
        //     with the language server.
        [DataMember(Name = "allCommitCharacters")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] AllCommitCharacters { get; set; }
        //
        // Summary:
        //     Gets or sets the trigger characters.
        [DataMember(Name = "triggerCharacters")]
        public string[] TriggerCharacters { get; set; }
    }
}
