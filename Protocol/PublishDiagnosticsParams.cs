using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter that's sent with 'textDocument/publishDiagnostics'
    //     messages.
    [DataContract]
    public class PublishDiagnosticParams
    {
        public PublishDiagnosticParams() { }

        //
        // Summary:
        //     Gets or sets the URI of the text document.
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
        //
        // Summary:
        //     Gets or sets the collection of diagnostics.
        [DataMember(Name = "diagnostics")]
        public Diagnostic[] Diagnostics { get; set; }
    }
}
