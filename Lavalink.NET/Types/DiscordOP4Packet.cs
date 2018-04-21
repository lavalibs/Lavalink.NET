using System;
using Newtonsoft.Json;

namespace Lavalink.NET.Player
{
	public class DiscordVoicePacket
	{
		[JsonProperty("guild_id")]
		public string GuildID { get; set; }
		[JsonProperty("channel_id")]
		public string ChannelID { get; set; }
		[JsonProperty("self_mute")]
		public bool SelfMute { get; set; }
		[JsonProperty("self_deaf")]
		public bool SelfDeaf { get; set; }

		public DiscordVoicePacket(string guildID, string channelID, bool selfMute, bool selfDeaf)
		{
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			ChannelID = channelID;
			SelfMute = selfMute;
			SelfDeaf = selfDeaf;
		}
	}

	public class DiscordOP4Packet
	{
		[JsonProperty("op")]
		public int OPCode { get; set; }
		[JsonProperty("d")]
		public DiscordVoicePacket DiscordVoicePacket { get; set; }

		public DiscordOP4Packet(DiscordVoicePacket packet)
		{
			OPCode = 4;
			DiscordVoicePacket = packet;
		}
	}
}
