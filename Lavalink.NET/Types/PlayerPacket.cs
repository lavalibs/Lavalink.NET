
using System;
using Newtonsoft.Json;

namespace Lavalink.NET.Player
{
	public class VoiceServerUpdate
	{
		[JsonProperty("guild_id")]
		public string GuildID { get; set; }
		[JsonProperty("token")]
		public string Token { get; set; }
		[JsonProperty("endpoint")]
		public string Endpoint { get; set; }

		public VoiceServerUpdate(string guildID, string token, string endpoint)
		{
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			Token = token ?? throw new ArgumentNullException(nameof(token));
			Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
		}
	}

	public class VoiceStateUpdate
	{
		[JsonProperty("guild_id")]
		public string GuildID { get; set; }
		[JsonProperty("channel_id")]
		public string ChannelID { get; set; }
		[JsonProperty("user_id")]
		public string UserID { get; set; }
		[JsonProperty("session_id")]
		public string SessionID { get; set; }
		[JsonProperty("deaf")]
		public bool Deaf { get; set; }
		[JsonProperty("mute")]
		public bool Mute { get; set; }
		[JsonProperty("self_deaf")]
		public bool SelfDeaf { get; set; }
		[JsonProperty("self_mute")]
		public bool SelfMute { get; set; }
		[JsonProperty("suppress")]
		public bool Suppress { get; set; }

		public VoiceStateUpdate(string guildID, string channelID, string userID, string sessionID, bool deaf, bool mute, bool selfDeaf, bool selfMute, bool suppress)
		{
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			ChannelID = channelID ?? throw new ArgumentNullException(nameof(channelID));
			UserID = userID ?? throw new ArgumentNullException(nameof(userID));
			SessionID = sessionID ?? throw new ArgumentNullException(nameof(sessionID));
			Deaf = deaf;
			Mute = mute;
			SelfDeaf = selfDeaf;
			SelfMute = selfMute;
			Suppress = suppress;
		}
	}

	public class PlayerPacket
	{
		[JsonProperty("op")]
		public string OPCode { get; set; }
		[JsonProperty("guildId")]
		public string GuildID { get; set; }
	}

	public class VoiceUpdatePacket : PlayerPacket
	{
		[JsonProperty("sessionId")]
		public string SessionID { get; set; }
		[JsonProperty("event")]
		public VoiceServerUpdate UpdateEvent { get; set; }

		public VoiceUpdatePacket(string opCode, string guildID, string sessionID, VoiceServerUpdate updateEvent)
		{
			OPCode = opCode ?? throw new ArgumentNullException(nameof(opCode));
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			SessionID = sessionID ?? throw new ArgumentNullException(nameof(sessionID));
			UpdateEvent = updateEvent ?? throw new ArgumentNullException(nameof(updateEvent));
		}
	}

	public class PlayPacket : PlayerPacket
	{
		[JsonProperty("track")]
		public string Track { get; set; }
		[JsonProperty("startTime")]
		public string StartTime { get; set; }
		[JsonProperty("endTime")]
		public string EndTime { get; set; }

		public PlayPacket(string opCode, string guildID, string track, string startTime, string endTime)
		{
			OPCode = opCode ?? throw new ArgumentNullException(nameof(opCode));
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			Track = track ?? throw new ArgumentNullException(nameof(track));
			StartTime = startTime ?? throw new ArgumentNullException(nameof(startTime));
			EndTime = endTime ?? throw new ArgumentNullException(nameof(endTime));
		}
	}

	public class PausePacket : PlayerPacket
	{
		[JsonProperty("pause")]
		public bool Paused { get; set; }

		public PausePacket(string opCode, string guildID, bool paused)
		{
			OPCode = opCode ?? throw new ArgumentNullException(nameof(opCode));
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			Paused = paused;
		}
	}

	public class SeekPacket : PlayerPacket
	{
		[JsonProperty("position")]
		public int Position { get; set; }

		public SeekPacket(string opCode, string guildID, int position)
		{
			OPCode = opCode ?? throw new ArgumentNullException(nameof(opCode));
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			Position = position;
		}
	}

	public class VolumePacket : PlayerPacket
	{
		[JsonProperty("volume")]
		public int Volume { get; set; }

		public VolumePacket(string opCode, string guildID, int volume)
		{
			OPCode = opCode ?? throw new ArgumentNullException(nameof(opCode));
			GuildID = guildID ?? throw new ArgumentNullException(nameof(guildID));
			Volume = volume;
		}
	}
}
