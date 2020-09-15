using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for on type formatting.
    [DataContract]
    public class DocumentOnTypeFormattingOptions
    {
        public DocumentOnTypeFormattingOptions() { }

        //
        // Summary:
        //     Gets or sets the first trigger character.
        [DataMember(Name = "firstTriggerCharacter")]
        public string FirstTriggerCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets additional trigger characters.
        [DataMember(Name = "moreTriggerCharacter")]
        public string[] MoreTriggerCharacter { get; set; }
    }
}
