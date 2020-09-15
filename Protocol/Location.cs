using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class representing a location in a document.
    [DataContract]
    public class Location : IEquatable<Location>
    {
        public Location() { }

        //
        // Summary:
        //     Gets or sets the URI for the document the location belongs to.
        [DataMember(Name = "uri")]
        [JsonProperty(Required = Required.Always)]
        public string Uri { get; set; }
        //
        // Summary:
        //     Gets or sets the range of the location in the document.
        [DataMember(Name = "range")]
        public Range Range { get; set; }

        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(Location other) { throw new NotImplementedException(); }
        public override int GetHashCode() { throw new NotImplementedException(); }
    }
}
