using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the initialization setting for completion item kind
    [DataContract]
    public class CompletionItemKindSetting
    {
        public CompletionItemKindSetting() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.CompletionItemKind
        //     values that the client supports.
        [DataMember(Name = "valueSet")]
        public CompletionItemKind[] ValueSet { get; set; }
    }
}
