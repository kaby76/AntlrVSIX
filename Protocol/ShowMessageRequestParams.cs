using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents parameter sent with window/showMessageRequest requests.
    [DataContract]
    public class ShowMessageRequestParams : ShowMessageParams
    {
        public ShowMessageRequestParams() { }

        //
        // Summary:
        //     Gets or sets an array of Microsoft.VisualStudio.LanguageServer.Protocol.MessageActionItems
        //     to present.
        [DataMember(Name = "actions")]
        public MessageActionItem[] Actions { get; set; }
    }
}
