using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkDoneProgressBegin
    {
        public WorkDoneProgressBegin() { }

        // The type of the Markup
        [DataMember(Name = "kind")]
        [JsonProperty(Required = Required.Always)]
        public string Kind { get; set; }

        /**
	     * Mandatory title of the progress operation. Used to briefly inform about
	     * the kind of operation being performed.
	     *
	     * Examples: "Indexing" or "Linking dependencies".
	     */
        [DataMember(Name = "title")]
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /**
	     * Controls if a cancel button should show to allow the user to cancel the
	     * long running operation. Clients that don't support cancellation are allowed
	     * to ignore the setting.
	     */
        [DataMember(Name = "cancellable")]
        [JsonProperty(Required = Required.Default)]
        public bool Cancellable { get; set; }

        /**
         * Optional, more detailed associated progress message. Contains
         * complementary information to the `title`.
         *
         * Examples: "3/25 files", "project/src/module2", "node_modules/some_dep".
         * If unset, the previous progress message (if any) is still valid.
         */
        [DataMember(Name = "message")]
        [JsonProperty(Required = Required.Default)]
        public string Message { get; set; }

        /**
         * Optional progress percentage to display (value 100 is considered 100%).
         * If not provided infinite progress is assumed and clients are allowed
         * to ignore the `percentage` value in subsequent in report notifications.
         *
         * The value should be steadily rising. Clients are free to ignore values
         * that are not following this rule.
         */
        [DataMember(Name = "percentage")]
        [JsonProperty(Required = Required.Default)]
        public int Percentage { get; set; }
    }
}
