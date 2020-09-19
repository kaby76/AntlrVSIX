using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class containing the set of code action kinds that are supported.
    [DataContract]
    public class CodeActionKindSetting
    {
        public CodeActionKindSetting() { }

        //
        // Summary:
        //     Gets or sets the code actions kinds the client supports.
        [DataMember(Name = "valueSet")]
        public CodeActionKind[] ValueSet { get; set; }
    }
}
