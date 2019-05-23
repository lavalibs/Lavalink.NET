using Newtonsoft.Json;
using Spectacles.NET.Types;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The VoiceUpdatePacket for the Player.
	/// </summary>
	public class VoiceUpdatePacket : PlayerPacket
	{
		/// <summary>
		/// The SessionID of this VoiceUpdatePacket
		/// </summary>
		[JsonProperty("sessionId")]
		public string SessionID { get; set; }

		/// <summary>
		/// The UpdateEvent of this VoiceUpdatePacket
		/// </summary>
		[JsonProperty("event")]
		public VoiceServerUpdatePayload UpdateEvent { get; set; }
	}
}