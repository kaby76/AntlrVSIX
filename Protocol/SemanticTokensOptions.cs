using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class SemanticTokensOptions : WorkDoneProgressOptions
    {
        [DataMember(Name = "legend")]
        public SemanticTokensLegend legend { get; set; }

        [DataMember(Name = "range")]
        public bool range { get; set; }

        [DataMember(Name = "full")] 
        public bool full { get; set; }
    }
}
