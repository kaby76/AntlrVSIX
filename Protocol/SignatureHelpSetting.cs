using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the signature help initialization setting.
    [DataContract]
    public class SignatureHelpSetting : DynamicRegistrationSetting
    {
        public SignatureHelpSetting() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SignatureInformationSetting
        //     information.
        [DataMember(Name = "signatureInformation")]
        public SignatureInformationSetting SignatureInformation { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether additional context information is supported
        //     for the `textDocument/signatureHelp` request.
        [DataMember(Name = "contextSupport")]
        public bool ContextSupport { get; set; }
    }
}
