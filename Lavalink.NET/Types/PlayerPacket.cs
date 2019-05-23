using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The base packet of all Player related events.
	/// </summary>
	public class PlayerPacket : BasePacket
	{
		/// <summary>
		/// The GuildID of this PlayerPacket
		/// </summary>
		[JsonProperty("guildId")]
		public string GuildID { get; set; }
	}
}