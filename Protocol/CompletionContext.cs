using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing additional information about the content in which a completion
    //     request is triggered.
    [DataContract]
    public class CompletionContext
    {
        public CompletionContext() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.CompletionTriggerKind
        //     indicating how the completion was triggered.
        [DataMember(Name = "triggerKind")]
        public CompletionTriggerKind TriggerKind { get; set; }
        //
        // Summary:
        //     Gets or sets the character that triggered code completion.
        [DataMember(Name = "triggerCharacter")]
        public string TriggerCharacter { get; set; }
    }
}
