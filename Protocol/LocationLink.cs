using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class LocationLink
    {
        public LocationLink() { }

        //
        // Summary:
        //     Gets or sets the range of the location in the document.
        [DataMember(Name = "originSelectionRange")]
        public Range OriginSelectionRange { get; set; }
        //
        // Summary:
        //     Gets or sets the URI for the document the location belongs to.
        [DataMember(Name = "targetUri")]
        [JsonProperty(Required = Required.Always)]
        public string TargetUri { get; set; }

        //
        // Summary:
        // The full target range of this link.If the target for example is a symbol then target range is the
        // range enclosing this symbol not including leading/trailing whitespace but everything else
        // like comments. This information is typically used to highlight the range in the editor.[DataMember(Name = "targetRange")]
        [DataMember(Name = "targetRange")]
        [JsonProperty(Required = Required.Always)]
        public Range TargetRange { get; set; }

        //
        // The range that should be selected and revealed when this link is being followed, e.g the name of a function.
        // Must be contained by the the `targetRange`. See also `DocumentSymbol#range`
        [DataMember(Name = "targetSelectionRange")]
        [JsonProperty(Required = Required.Always)]
        public Range TargetSelectionRange { get; set; }
    }
}
