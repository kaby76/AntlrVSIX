using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents formatting options.
    [DataContract]
    public class FormattingOptions
    {
        public FormattingOptions() { }

        //
        // Summary:
        //     Gets or sets the number of spaces to be inserted per tab.
        [DataMember(Name = "tabSize")]
        public int TabSize { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether tabs should be spaces.
        [DataMember(Name = "insertSpaces")]
        public bool InsertSpaces { get; set; }
        //
        // Summary:
        //     Gets or sets the other potential formatting options.
        [JsonExtensionData]
        public Dictionary<string, object> OtherOptions { get; set; }
    }
}
