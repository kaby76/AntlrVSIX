using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    /**
     * Static registration options to be returned in the initialize request.
     */
    [DataContract]
    public class StaticRegistrationOptions
    {
        public StaticRegistrationOptions() { }

        /**
         * The id used to register the request. The id can be used to deregister
         * the request again. See also Registration#id.
         */
        [DataMember(Name = "id")]
        [JsonProperty(Required = Required.Default)]
        string id { get; set; }
    }
}
