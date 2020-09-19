using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents initialization setting for completion item.
    [DataContract]
    public class CompletionItemSetting
    {
        public CompletionItemSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether completion items can contain snippets.
        [DataMember(Name = "snippetSupport")]
        public bool SnippetSupport { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the client supports commit characters.
        [DataMember(Name = "commitCharactersSupport")]
        public bool CommitCharactersSupport { get; set; }
        //
        // Summary:
        //     Gets or sets the content formats supported for documentation.
        [DataMember(Name = "documentationFormat")]
        public MarkupKind[] DocumentationFormat { get; set; }
    }
}
