using System.Runtime.Serialization;


namespace Server
{
    /**
     * Static registration options to be returned in the initialize request.
     */
    [DataContract]
    public class StaticRegistrationOptions
    {
        /**
         * The id used to register the request. The id can be used to deregister
         * the request again. See also Registration#id.
         */
        string id { get; set; }
    }
}
