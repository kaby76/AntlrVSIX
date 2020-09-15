using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the options for execute command support.
    [DataContract]
    public class ExecuteCommandOptions
    {
        public ExecuteCommandOptions() { }

        //
        // Summary:
        //     Gets or sets the commands that are to be executed on the server.
        [DataMember(Name = "commands")]
        public string[] Commands { get; set; }
    }
}
