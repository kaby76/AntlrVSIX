using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing additional information about the context in which a signature
    //     help request is triggered.
    [DataContract]
    public class SignatureHelpContext
    {
        public SignatureHelpContext() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SignatureHelpTriggerKind
        //     indicating how the signature help was triggered.
        [DataMember(Name = "triggerKind")]
        public SignatureHelpTriggerKind TriggerKind { get; set; }
        //
        // Summary:
        //     Gets or sets the character that caused signature help to be triggered. This value
        //     is null when triggerKind is not TriggerCharacter.
        [DataMember(Name = "triggerCharacter")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TriggerCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether signature help was already showing when
        //     it was triggered.
        [DataMember(Name = "isRetrigger")]
        public bool IsRetrigger { get; set; }
        //
        // Summary:
        //     Gets or sets the currently active Microsoft.VisualStudio.LanguageServer.Protocol.SignatureHelp.
        [DataMember(Name = "activeSignatureHelp")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SignatureHelp ActiveSignatureHelp { get; set; }
    }
}
