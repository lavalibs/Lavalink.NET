using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Player EndEvent Packet.
	/// </summary>
	public class PlayerEndEventPacket : PlayerTrackEventPacket
	{
		/// <summary>
		/// The Reason the Track ended
		/// </summary>
		[JsonProperty("reason")]
		public string Reason { get; set; }
	}
}