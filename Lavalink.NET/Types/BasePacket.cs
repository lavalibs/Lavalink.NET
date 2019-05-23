using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Base Packet of all Lavalink Events
	/// </summary>
	public class BasePacket
	{
		/// <summary>
		/// The OPCode of this PlayerPacket
		/// </summary>
		[JsonProperty("op")]
		public string OPCode { get; set; }
	}
}