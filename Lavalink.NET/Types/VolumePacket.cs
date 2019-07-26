using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The VolumePacket for the Player.
	/// </summary>
	public class VolumePacket : PlayerPacket
	{
		/// <summary>
		/// The volume to change to.
		/// </summary>
		[JsonProperty("volume")]
		public int Volume { get; set; }
	}
}