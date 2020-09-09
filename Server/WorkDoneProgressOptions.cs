using System.Runtime.Serialization;


namespace Server
{
    [DataContract]
    public class WorkDoneProgressOptions
    {
        [DataMember(Name = "workDoneProgress")]
        public bool WorkDoneProgress { get; set; }
    }
}
