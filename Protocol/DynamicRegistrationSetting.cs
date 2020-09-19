using System;
using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents a setting that can be dynamically registered.
    [DataContract]
    public class DynamicRegistrationSetting
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.DynamicRegistrationSetting
        //     class.
        public DynamicRegistrationSetting() { }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.DynamicRegistrationSetting
        //     class.
        //
        // Parameters:
        //   value:
        //     Value indicating whether the setting can be dynamically registered.
        public DynamicRegistrationSetting(bool value) { throw new NotImplementedException(); }

        //
        // Summary:
        //     Gets or sets a value indicating whether setting can be dynamically registered.
        [DataMember(Name = "dynamicRegistration")]
        public bool DynamicRegistration { get; set; }
    }
}
