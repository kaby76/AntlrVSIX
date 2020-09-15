using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents save option configurations.
    [DataContract]
    public class SaveOptions
    {
        public SaveOptions() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether clients include text content on save.
        [DataMember(Name = "includeText")]
        public bool IncludeText { get; set; }
    }
}
