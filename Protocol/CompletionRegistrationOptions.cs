using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for registering completion support.
    [DataContract]
    public class CompletionRegistrationOptions : TextDocumentRegistrationOptions
    {
        public CompletionRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets the characters that trigger a completion session.
        [DataMember(Name = "triggerCharacters")]
        public string[] TriggerCharacters { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the server provides resolve support.
        [DataMember(Name = "resolveProvider")]
        public bool ResolveProvider { get; set; }
    }
}
