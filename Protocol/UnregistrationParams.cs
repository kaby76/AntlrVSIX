using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameter sent for the client/unregisterCapability request.
    [DataContract]
    public class UnregistrationParams
    {
        public UnregistrationParams() { }

        //
        // Summary:
        //     Gets or sets the capabilities to unregister.
        [DataMember(Name = "unregistrations")]
        public Unregistration[] Unregistrations { get; set; }
    }
}
