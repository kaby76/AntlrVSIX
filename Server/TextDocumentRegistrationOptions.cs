using System.Runtime.Serialization;


namespace Server
{
    [DataContract]
    public class TextDocumentRegistrationOptions
    {
        /**
         * A document selector to identify the scope of the registration. If set to null
         * the document selector provided on the client side will be used.
         */
        public DocumentFilter[] documentSelector { get; set; }
    }
}
