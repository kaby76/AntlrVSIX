using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
    [DataContract]
    public class WorkspaceFolder
    {
        public WorkspaceFolder() { }

		/**
		 * The associated URI for this workspace folder.
		 */
		[DataMember(Name = "uri")]
		[JsonProperty(Required = Required.Always)]
		public string Uri { get; set; }

		/**
		 * The name of the workspace folder. Used to refer to this
		 * workspace folder in the user interface.
		 */
		[DataMember(Name = "name")]
		[JsonProperty(Required = Required.Always)]
		public string Name { get; set; }
    }
}
