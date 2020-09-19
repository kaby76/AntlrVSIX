using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent from client to server for the workspace/executeCommand
    //     request.
    [DataContract]
    public class ExecuteCommandParams
    {
        public ExecuteCommandParams() { }

        //
        // Summary:
        //     Gets or sets the command identifier associated with the command handler.
        [DataMember(Name = "command")]
        public string Command { get; set; }
        //
        // Summary:
        //     Gets or sets the arguments that the command should be invoked with.
        [DataMember(Name = "arguments")]
        public object[] Arguments { get; set; }
    }
}
