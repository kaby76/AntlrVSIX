using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Protocol
{
	[DataContract]
	public class WorkspaceFoldersChangeEvent
	{
		public WorkspaceFoldersChangeEvent() { }

		/**
		 * The array of added workspace folders
		 */
		[DataMember(Name = "added")]
		[JsonProperty(Required = Required.Always)]
		public WorkspaceFolder[] Added { get; set; }


		/**
		 * The array of the removed workspace folders
		 */
		[DataMember(Name = "removed")]
		[JsonProperty(Required = Required.Always)]
		public WorkspaceFolder[] Removed { get; set; }
	}
}
