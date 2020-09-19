using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the parameters sent for the client/registerCapability request.
    [DataContract]
    public class RegistrationParams
    {
        public RegistrationParams() { }

        //
        // Summary:
        //     Gets or sets the set of capabilities that are being registered.
        [DataMember(Name = "registrations")]
        public Registration[] Registrations { get; set; }
    }
}
