using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the initialization setting for hover.
    [DataContract]
    public class HoverSetting : DynamicRegistrationSetting
    {
        public HoverSetting() { }

        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.MarkupKind values
        //     supported.
        [DataMember(Name = "contentFormat")]
        public MarkupKind[] ContentFormat { get; set; }
    }
}
