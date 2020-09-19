using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent for a textDocument/onTypeFormatting request.
    [DataContract]
    public class DocumentOnTypeFormattingParams
    {
        public DocumentOnTypeFormattingParams() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentIdentifier
        //     representing the document to format.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.DocumentOnTypeFormattingParams.Position
        //     at which the request was sent.
        [DataMember(Name = "position")]
        public Position Position { get; set; }
        //
        // Summary:
        //     Gets or sets the character that was typed.
        [DataMember(Name = "ch")]
        public string Character { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.FormattingOptions
        //     for the request.
        [DataMember(Name = "options")]
        public FormattingOptions Options { get; set; }
    }
}
