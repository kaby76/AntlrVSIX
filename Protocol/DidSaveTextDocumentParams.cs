using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that is sent with a textDocument/didSave
    //     message.
    [DataContract]
    public class DidSaveTextDocumentParams
    {
        public DidSaveTextDocumentParams() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentIdentifier
        //     which represents the text document that was saved.
        [DataMember(Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        //
        // Summary:
        //     Gets or sets the System.String which represents the content of the text document
        //     when it was saved.
        [DataMember(Name = "text")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }
}
