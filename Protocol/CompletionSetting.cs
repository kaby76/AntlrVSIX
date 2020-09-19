using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents initialization setting for completion.
    [DataContract]
    public class CompletionSetting : DynamicRegistrationSetting
    {
        public CompletionSetting() { }

        //
        // Summary:
        //     Gets or sets completion item setting.
        [DataMember(Name = "completionItem")]
        public CompletionItemSetting CompletionItem { get; set; }
        //
        // Summary:
        //     Gets or sets Microsoft.VisualStudio.LanguageServer.Protocol.CompletionItemKind
        //     specific settings.
        [DataMember(Name = "completionItemKind")]
        public CompletionItemKindSetting CompletionItemKind { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the client supports sending additional
        //     context.
        [DataMember(Name = "contextSupport")]
        public bool ContextSupport { get; set; }
    }
}
