using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the general registration information for registering for a
    //     capability.
    [DataContract]
    public class Registration
    {
        public Registration() { }

        //
        // Summary:
        //     Gets or sets the id used to register the request. This can be used to deregister
        //     later.
        [DataMember(Name = "id")]
        public string Id { get; set; }
        //
        // Summary:
        //     Gets or sets the method / capability to register for.
        [DataMember(Name = "method")]
        public string Method { get; set; }
        //
        // Summary:
        //     Gets or sets the options necessary for registration.
        [DataMember(Name = "registerOptions")]
        public object RegisterOptions { get; set; }
    }
}
