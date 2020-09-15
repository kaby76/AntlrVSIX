using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for signature help support.
    [DataContract]
    public class SignatureHelpOptions
    {
        public SignatureHelpOptions() { }

        //
        // Summary:
        //     Gets or sets the characters that trigger signature help automatically.
        [DataMember(Name = "triggerCharacters")]
        public string[] TriggerCharacters { get; set; }
        //
        // Summary:
        //     Gets or sets the characters that re-trigger signature help when signature help
        //     is already showing.
        [DataMember(Name = "retriggerCharacters")]
        public string[] RetriggerCharacters { get; set; }
    }
}
