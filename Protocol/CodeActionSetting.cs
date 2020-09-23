using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //
    // Summary:
    //     Class representing settings for code action support.
    [DataContract]
    public class CodeActionSetting : DynamicRegistrationSetting
    {
        public CodeActionSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating the client supports code action literals.
        [DataMember(Name = "codeActionLiteralSupport")]
        public CodeActionLiteralSetting CodeActionLiteralSupport { get; set; }
    }
}
