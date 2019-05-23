using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The SeekPacket for the Player.
	/// </summary>
	public class SeekPacket : PlayerPacket
	{
		/// <summary>
		/// The Position the track should seek to.
		/// </summary>
		[JsonProperty("position")]
		public long Position { get; set; }
	}
}