using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Player Event based on Track Event
	/// </summary>
	public class PlayerTrackEventPacket : PlayerEventPacket
	{
		/// <summary>
		/// The Track which ended playing
		/// </summary>
		[JsonProperty("track")]
		public string Track { get; set; }
	}
}