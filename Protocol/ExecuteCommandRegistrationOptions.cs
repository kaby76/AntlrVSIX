using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the registration options for indicating execute command support.
    [DataContract]
    public class ExecuteCommandRegistrationOptions
    {
        public ExecuteCommandRegistrationOptions() { }

        //
        // Summary:
        //     Gets or sets an array of commands that are to be executed on the server.
        [DataMember(Name = "commands")]
        public string[] Commands { get; set; }
    }
}
