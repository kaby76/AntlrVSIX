using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for registering textDocument/didSave support.
    [DataContract]
    public class TextDocumentSaveRegistrationOptions : TextDocumentRegistrationOptions
    {
        public TextDocumentSaveRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether the client should include the content
        //     on save.
        [DataMember(Name = "includeText")]
        public bool IncludeText { get; set; }
    }
}
