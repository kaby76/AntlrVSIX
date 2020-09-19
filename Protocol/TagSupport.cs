using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the Microsoft.VisualStudio.LanguageServer.Protocol.TagSupport
    //     capabilities.
    [DataContract]
    public class TagSupport
    {
        public TagSupport() { }

        //
        // Summary:
        //     Gets or sets a value indicating the tags supported by the client.
        [DataMember(Name = "valueSet")]
        public DiagnosticTag[] ValueSet { get; set; }
    }
}
