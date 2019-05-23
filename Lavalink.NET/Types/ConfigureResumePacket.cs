using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Packet to Configure Resuming for a Lavalink Node.
	/// </summary>
	public class ConfigureResumePacket : BasePacket
	{
		/// <summary>
		/// The string you will need to send when resuming the session. Set to null to disable resuming altogether. 
		/// </summary>
		[JsonProperty("key")]
		public string Key { get; set; }
		
		/// <summary>
		/// The number of seconds after disconnecting before the session is closed anyways. This is useful for avoiding accidental leaks. 
		/// </summary>
		[JsonProperty("timeout")]
		public int Timeout { get; set; }
	}
}