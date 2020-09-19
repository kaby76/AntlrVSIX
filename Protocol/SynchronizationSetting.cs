using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Class which represents synchronization initialization setting.
    [DataContract]
    public class SynchronizationSetting : DynamicRegistrationSetting
    {
        public SynchronizationSetting() { }

        //
        // Summary:
        //     Gets or sets a value indicating whether WillSave event is supported.
        [DataMember(Name = "willSave")]
        public bool WillSave { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether WillSaveWaitUntil event is supported.
        [DataMember(Name = "willSaveWaitUntil")]
        public bool WillSaveWaitUntil { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether DidSave event is supported.
        [DataMember(Name = "didSave")]
        public bool DidSave { get; set; }
    }
}
