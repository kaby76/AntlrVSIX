using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkDoneProgressOptions
    {
        [DataMember(Name = "workDoneProgress")]
        public bool WorkDoneProgress { get; set; }
    }
}
