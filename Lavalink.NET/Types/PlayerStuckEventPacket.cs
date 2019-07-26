using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Player StuckEvent Packet.
	/// </summary>
	public class PlayerStuckEventPacket : PlayerTrackEventPacket
	{
		/// <summary>
		/// The position the song ended
		/// </summary>
		[JsonProperty("thresholdMs")]
		public long Threshold { get; set; }
	}
}