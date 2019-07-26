using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The PlayerUpdate for the Player
	/// </summary>
	public class PlayerUpdatePacket : PlayerPacket
	{
		/// <summary>
		/// The State of the Player
		/// </summary>
		[JsonProperty("state")]
		public PlayerState State { get; set; }
	}
}