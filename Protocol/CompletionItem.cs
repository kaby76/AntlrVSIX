using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents an IntelliSense completion item.
    [DataContract]
    public class CompletionItem
    {
        public CompletionItem()
        {
            Kind = CompletionItemKind.Text;
            InsertTextFormat = InsertTextFormat.Plaintext;
        }

        //
        // Summary:
        //     Gets or sets the label value, i.e. display text to users.
        [DataMember(Name = "label")]
        public string Label { get; set; }
        //
        // Summary:
        //     Gets or sets the completion kind.
        [DataMember(Name = "kind")]
        public CompletionItemKind Kind { get; set; }
        //
        // Summary:
        //     Gets or sets the completion detail.
        [DataMember(Name = "detail")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }
        //
        // Summary:
        //     Gets or sets the documentation comment.
        [DataMember(Name = "documentation")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SumType<string, MarkupContent> Documentation { get; set; }
        [DataMember(Name = "preselect")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Preselect { get; set; }
        //
        // Summary:
        //     Gets or sets the custom sort text.
        [DataMember(Name = "sortText")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SortText { get; set; }
        //
        // Summary:
        //     Gets or sets the custom filter text.
        [DataMember(Name = "filterText")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FilterText { get; set; }
        //
        // Summary:
        //     Gets or sets the insert text.
        [DataMember(Name = "insertText")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InsertText { get; set; }
        //
        // Summary:
        //     Gets or sets the insert text format.
        [DataMember(Name = "insertTextFormat")]
        [DefaultValue(InsertTextFormat.Plaintext)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InsertTextFormat InsertTextFormat { get; set; }
        //
        // Summary:
        //     Gets or sets the text edit.
        [DataMember(Name = "textEdit")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TextEdit TextEdit { get; set; }
        //
        // Summary:
        //     Gets or sets any additional text edits.
        //
        // Remarks:
        //     Additional text edits must not interfere with the main text edit.
        [DataMember(Name = "additionalTextEdits")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TextEdit[] AdditionalTextEdits { get; set; }
        //
        // Summary:
        //     Gets or sets the set of characters that will trigger completion when pressed
        //     and then type the character.
        [DataMember(Name = "commitCharacters")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] CommitCharacters { get; set; }
        //
        // Summary:
        //     Gets or sets any optional command that will be executed after completion item
        //     insertion.
        //
        // Remarks:
        //     This feature is not supported in VS.
        [DataMember(Name = "command")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Command Command { get; set; }
        //
        // Summary:
        //     Gets or sets any additional data that links the unresolve completion item and
        //     the resolved completion item.
        [DataMember(Name = "data")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}
