using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Player Packet for a WebSocketClosedEvent
	/// </summary>
	public class PlayerCloseEventPacket : PlayerEventPacket
	{
		/// <summary>
		/// The Close Code if the WebSocket
		/// </summary>
		[JsonProperty("code")]
		public int CloseCode { get; set; }
		
		/// <summary>
		/// The Close Reason in text
		/// </summary>
		[JsonProperty("reason")]
		public string Reason { get; set; }
		
		/// <summary>
		/// If this Connection
		/// </summary>
		[JsonProperty("byRemote")]
		public bool ByRemote { get; set; }
	}
}