using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the signature of something callable. This class is returned
    //     from the textDocument/signatureHelp request.
    [DataContract]
    public class SignatureHelp
    {
        public SignatureHelp() { }

        //
        // Summary:
        //     Gets or sets an array of signatures associated with the callable item.
        [DataMember(Name = "signatures")]
        public SignatureInformation[] Signatures { get; set; }
        //
        // Summary:
        //     Gets or sets the active signature. If the value is omitted or falls outside the
        //     range of Signatures it defaults to zero.
        [DataMember(Name = "activeSignature")]
        public int ActiveSignature { get; set; }
        //
        // Summary:
        //     Gets or sets the active parameter. If the value is omitted or falls outside the
        //     range of Signatures[ActiveSignature].Parameters it defaults to zero.
        [DataMember(Name = "activeParameter")]
        public int ActiveParameter { get; set; }
    }
}
