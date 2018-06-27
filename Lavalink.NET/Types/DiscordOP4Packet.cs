using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The VoicePacket for Discord
	/// </summary>
	public class DiscordVoicePacket
	{
		/// <summary>
		/// The GuildID of the VoicePacket as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong GuildID { get; set; }

		/// <summary>
		/// The GuildID of the VoicePacket as String.
		/// </summary>
		[JsonProperty("guild_id")]
		public string GuildIDString => GuildID.ToString();

		/// <summary>
		/// The ChannelID of the VoicePacket as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong? ChannelID { get; set; }

		/// <summary>
		/// The ChannelID of the VoicePacket as String.
		/// </summary>
		[JsonProperty("channel_id")]
		public string ChannelIDString => ChannelID?.ToString();

		/// <summary>
		/// Should the Bot be muted.
		/// </summary>
		[JsonProperty("self_mute")]
		public bool SelfMute { get; set; } = false;

		/// <summary>
		/// Should the Bot be deafend.
		/// </summary>
		[JsonProperty("self_deaf")]
		public bool SelfDeaf { get; set; } = false;
	}

	/// <summary>
	/// The complete OP4 Packet for Discord
	/// </summary>
	public class DiscordOP4Packet
	{
		/// <summary>
		/// The OP Code for Discord websocket. (always 4 for voice)
		/// </summary>
		[JsonProperty("op")]
		public int OPCode { get; set; } = 4;

		/// <summary>
		/// The DiscordVoicePacket to send to the Discord Websocket Gateway.
		/// </summary>
		[JsonProperty("d")]
		public DiscordVoicePacket DiscordVoicePacket { get; set; }

		/// <summary>
		/// Creates a new DiscordOP4Packet instance.
		/// </summary>
		/// <param name="guildID">The GuildID to take action on.</param>
		/// <param name="channelID">The channel to take action on (for Leave this is null).</param>
		/// <param name="deaf">Should the Bot join deafend.</param>
		/// <param name="mute">Should the Bot join muted.</param>
		public DiscordOP4Packet(ulong guildID, ulong? channelID, bool mute, bool deaf)
		{
			DiscordVoicePacket = new DiscordVoicePacket {
				GuildID = guildID,
				ChannelID = channelID,
				SelfMute = mute,
				SelfDeaf = deaf
			};
		}
	}
}
