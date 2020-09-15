using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents the parameter sent with workspace/didChangeConfiguration
    //     requests.
    [DataContract]
    public class DidChangeConfigurationParams
    {
        public DidChangeConfigurationParams() { }

        //
        // Summary:
        //     Gets or sets the settings that are applicable to the language server.
        [DataMember(Name = "settings")]
        public object Settings { get; set; }
    }
}
