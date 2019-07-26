using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Player ExceptionEvent Packet.
	/// </summary>
	public class PlayerExceptionEventPacket : PlayerTrackEventPacket
	{
		/// <summary>
		/// The Reason the Track errored
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }
	}
}