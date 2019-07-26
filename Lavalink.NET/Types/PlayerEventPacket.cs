using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Base Packet for Player based Events
	/// </summary>
	public class PlayerEventPacket : PlayerPacket
	{
		/// <summary>
		/// The PlayerEvent Type of this Packet
		/// </summary>
		[JsonProperty("type")]
		public PlayerEventType EventType { get; set; }
	}
}