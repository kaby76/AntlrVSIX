using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the registration options for indicating signature help support.
    [DataContract]
    public class SignatureHelpRegistrationOptions : TextDocumentRegistrationOptions
    {
        public SignatureHelpRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets the characters that trigger signature help automatically.
        [DataMember(Name = "triggerCharacters")]
        public string[] TriggerCharacters { get; set; }
    }
}
