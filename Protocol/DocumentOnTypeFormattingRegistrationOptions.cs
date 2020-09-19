using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for registering textDocument/onTypeFormatting
    //     support.
    [DataContract]
    public class DocumentOnTypeFormattingRegistrationOptions : TextDocumentRegistrationOptions
    {
        public DocumentOnTypeFormattingRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets the trigger character that should trigger formatting.
        [DataMember(Name = "firstTriggerCharacter")]
        public string FirstTriggerCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets additional trigger characters.
        [DataMember(Name = "moreTriggerCharacter")]
        public string[] MoreTriggerCharacter { get; set; }
    }
}
