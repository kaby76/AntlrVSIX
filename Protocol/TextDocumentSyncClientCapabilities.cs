using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Protocol
{
    [DataContract]
    public class TextDocumentSyncClientCapabilities
    {
        public TextDocumentSyncClientCapabilities() { }

        /**
	     * Whether text document synchronization supports dynamic registration.
	     */
        [DataMember(Name = "dynamicRegistration")]
        [JsonProperty(Required = Required.Default)]
        public bool dynamicRegistration { get; set; }

        /**
	     * The client supports sending will save notifications.
	     */
        [DataMember(Name = "willSave")]
        [JsonProperty(Required = Required.Default)]
        public bool WillSave { get; set; }

        /**
	     * The client supports sending a will save request and
	     * waits for a response providing text edits which will
	     * be applied to the document before it is saved.
	     */
        [DataMember(Name = "willSaveWaitUntil")]
        [JsonProperty(Required = Required.Default)]
        public bool WillSaveWaitUntil { get; set; }

        /**
	     * The client supports did save notifications.
	     */
        [DataMember(Name = "didSave")]
        [JsonProperty(Required = Required.Default)]
        public bool DidSave { get; set; }
    }
}
