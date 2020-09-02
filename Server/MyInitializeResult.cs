using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Server
{
    //
    // Summary:
    //     Class which represents the result returned by the initialize request.
    [DataContract]
    public class MyInitializeResult
    {
        public MyInitializeResult() { }

        //
        // Summary:
        //     Gets or sets the server capabilities.
        [DataMember(Name = "capabilities")]
        public MyServerCapabilities Capabilities { get; set; }
    }
}