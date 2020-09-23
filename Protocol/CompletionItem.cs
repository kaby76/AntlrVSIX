using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class CompletionItem
    {
        public CompletionItem()
        {
            Kind = CompletionItemKind.Text;
            InsertTextFormat = InsertTextFormat.Plaintext;
        }

        /**
	     * The label of this completion item. By default
	     * also the text that is inserted when selecting
	     * this completion.
	     */
        [DataMember(Name = "label")]
        [JsonProperty(Required = Required.Always)]
        public string Label { get; set; }

        /**
	     * The kind of this completion item. Based of the kind
	     * an icon is chosen by the editor. The standardized set
	     * of available values is defined in `CompletionItemKind`.
	     */
        [DataMember(Name = "kind")]
        [JsonProperty(Required = Required.Default)]
        public CompletionItemKind Kind { get; set; }

        /**
	     * Tags for this completion item.
	     *
	     * @since 3.15.0
	     */
        [DataMember(Name = "tags")]
        [JsonProperty(Required = Required.Default)]
        public CompletionItemTag[] Tags { get; set; }

        /**
	     * A human-readable string with additional information
	     * about this item, like type or symbol information.
	     */
        [DataMember(Name = "detail")]
        [JsonProperty(Required = Required.Default)]
        public string Detail { get; set; }

        /**
	     * A human-readable string that represents a doc-comment.
	     */
        [DataMember(Name = "documentation")]
        [JsonProperty(Required = Required.Default)]
        public SumType<string, MarkupContent> Documentation { get; set; }

        /**
	     * Indicates if this item is deprecated.
	     *
	     * @deprecated Use `tags` instead if supported.
	     */
        [DataMember(Name = "deprecated")]
        [JsonProperty(Required = Required.Default)]
        public bool Deprecated { get; set; }

        /**
	     * Select this item when showing.
	     *
	     * *Note* that only one completion item can be selected and that the
	     * tool / client decides which item that is. The rule is that the *first*
	     * item of those that match best is selected.
	     */
        [DataMember(Name = "preselect")]
        [JsonProperty(Required = Required.Default)]
        public bool Preselect { get; set; }

        /**
	     * A string that should be used when comparing this item
	     * with other items. When `falsy` the label is used.
	     */
        [DataMember(Name = "sortText")]
        [JsonProperty(Required = Required.Default)]
        public string SortText { get; set; }

        /**
	     * A string that should be used when filtering a set of
	     * completion items. When `falsy` the label is used.
	     */
        [DataMember(Name = "filterText")]
        [JsonProperty(Required = Required.Default)]
        public string FilterText { get; set; }

        /**
	     * A string that should be inserted into a document when selecting
	     * this completion. When `falsy` the label is used.
	     *
	     * The `insertText` is subject to interpretation by the client side.
	     * Some tools might not take the string literally. For example
	     * VS Code when code complete is requested in this example `con<cursor position>`
	     * and a completion item with an `insertText` of `console` is provided it
	     * will only insert `sole`. Therefore it is recommended to use `textEdit` instead
	     * since it avoids additional client side interpretation.
	     */
        [DataMember(Name = "insertText")]
        [JsonProperty(Required = Required.Default)]
        public string InsertText { get; set; }

        /**
	     * The format of the insert text. The format applies to both the `insertText` property
	     * and the `newText` property of a provided `textEdit`. If omitted defaults to
	     * `InsertTextFormat.PlainText`.
	     */
        [DataMember(Name = "insertTextFormat")]
        [DefaultValue(InsertTextFormat.Plaintext)]
        [JsonProperty(Required = Required.Default)]
        public InsertTextFormat InsertTextFormat { get; set; }

        /**
	     * An edit which is applied to a document when selecting this completion. When an edit is provided the value of
	     * `insertText` is ignored.
	     *
	     * *Note:* The range of the edit must be a single line range and it must contain the position at which completion
	     * has been requested.
	     */
        [DataMember(Name = "textEdit")]
        [JsonProperty(Required = Required.Default)]
        public TextEdit TextEdit { get; set; }

        /**
	     * An optional array of additional text edits that are applied when
	     * selecting this completion. Edits must not overlap (including the same insert position)
	     * with the main edit nor with themselves.
	     *
	     * Additional text edits should be used to change text unrelated to the current cursor position
	     * (for example adding an import statement at the top of the file if the completion item will
	     * insert an unqualified type).
	     */
        [DataMember(Name = "additionalTextEdits")]
        [JsonProperty(Required = Required.Default)]
        public TextEdit[] AdditionalTextEdits { get; set; }

        /**
	     * An optional set of characters that when pressed while this completion is active will accept it first and
	     * then type that character. *Note* that all commit characters should have `length=1` and that superfluous
	     * characters will be ignored.
	     */
        [DataMember(Name = "commitCharacters")]
        [JsonProperty(Required = Required.Default)]
        public string[] CommitCharacters { get; set; }

        /**
	     * An optional command that is executed *after* inserting this completion. *Note* that
	     * additional modifications to the current document should be described with the
	     * additionalTextEdits-property.
	     */
        [DataMember(Name = "command")]
        [JsonProperty(Required = Required.Default)]
        public Command Command { get; set; }

        /**
	     * A data entry field that is preserved on a completion item between
	     * a completion and a completion resolve request.
	     */
        [DataMember(Name = "data")]
        [JsonProperty(Required = Required.Default)]
        public object Data { get; set; }
    }
}
