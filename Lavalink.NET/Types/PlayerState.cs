using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The State of a Player
	/// </summary>
	public class PlayerState
	{
		/// <summary>
		/// The unix timestamp of the Player
		/// </summary>
		[JsonProperty("time")]
		public long Time { get; set; }
		
		/// <summary>
		/// The current position of the Player
		/// </summary>
		[JsonProperty("position")]
		public long Position { get; set; }
	}
}