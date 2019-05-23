using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The PlayPacket for the Player.
	/// </summary>
	public class PlayPacket : PlayerPacket
	{
		/// <summary>
		/// The Track string of this PlayPacket.
		/// </summary>
		[JsonProperty("track")]
		public string Track { get; set; }

		/// <summary>
		/// The Start time of this PlayPacket.
		/// </summary>
		[JsonProperty("startTime")]
		public string StartTime { get; set; }

		/// <summary>
		/// The End time of this PlayPacket.
		/// </summary>
		[JsonProperty("endTime")]
		public string EndTime { get; set; }
	}
}