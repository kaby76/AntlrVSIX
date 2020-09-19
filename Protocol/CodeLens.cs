using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     A class representing a code lens command that should be shown alongside source
    //     code.
    [DataContract]
    public class CodeLens
    {
        public CodeLens() { }

        //
        // Summary:
        //     Gets or sets the range that the code lens applies to.
        [DataMember(Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the command associated with this code lens.
        [DataMember(Name = "command")]
        public Command Command { get; set; }
        //
        // Summary:
        //     Gets or sets the data that should be preserved between a textDocument/codeLens
        //     request and a codeLens/resolve request.
        [DataMember(Name = "data")]
        public object Data { get; set; }
    }
}
