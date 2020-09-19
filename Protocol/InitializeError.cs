using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing the error type sent when the initialize request fails.
    [DataContract]
    public class InitializeError
    {
        public InitializeError() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether or not to retry.
        [DataMember(Name = "retry")]
        public bool Retry { get; set; }
    }
}
