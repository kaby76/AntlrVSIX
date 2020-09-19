using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the initialization setting for publish diagnostics.
    [DataContract]
    public class PublishDiagnosticsSetting
    {
        public PublishDiagnosticsSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.PublishDiagnosticsSetting.TagSupport
        //     capabilities.
        [DataMember(Name = "tagSupport")]
        public bool TagSupport { get; set; }
    }
}
