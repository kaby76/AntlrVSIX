using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    //
    // Summary:
    //     Class representing support for code action literals.
    [DataContract]
    public class CodeActionLiteralSetting
    {
        public CodeActionLiteralSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating what code action kinds are supported.
        [DataMember(Name = "codeActionKind")]
        public CodeActionKindSetting CodeActionKind { get; set; }
    }
}
