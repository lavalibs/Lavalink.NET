using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The PausePacket for the Player.
	/// </summary>
	public class PausePacket : PlayerPacket
	{
		/// <summary>
		/// Boolean indicating if the Player should be paused or not.
		/// </summary>
		[JsonProperty("pause")]
		public bool Pause { get; set; }
	}
}