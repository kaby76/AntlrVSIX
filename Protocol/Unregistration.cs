using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the information needed for unregistering a capability.
    [DataContract]
    public class Unregistration
    {
        public Unregistration() { }

        //
        // Summary:
        //     Gets or sets the id of the unregistration.
        [DataMember(Name = "id")]
        public string Id { get; set; }
        //
        // Summary:
        //     Gets or sets the method to unregister.
        [DataMember(Name = "method")]
        public string Method { get; set; }
    }
}
