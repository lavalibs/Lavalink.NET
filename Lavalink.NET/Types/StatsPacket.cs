using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The StatsPacket of Lavalink
	/// </summary>
	public class StatsPacket : LavalinkStats
	{
		/// <summary>
		/// The OPCode of this PlayerPacket
		/// </summary>
		[JsonProperty("op")]
		public string OPCode { get; set; }
	}
}